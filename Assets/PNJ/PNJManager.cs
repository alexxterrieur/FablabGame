using System;
using System.Collections;
using System.Collections.Generic;
using DeliveryPoint;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace PNJ
{
    public class PNJManager : MonoBehaviour
    {
        [Header("Orders")]
        [SerializeField] private List<SO_Order> orders = new();
        
        [Header("Waypoints Assemblers")]
        [SerializeField] private Transform plasticAssemblerWaypoint;
        [SerializeField] private Transform woodAssemblerWaypoint;
        [SerializeField] private Transform metalAssemblerWaypoint;
        
        [Header("Waypoints Collectors")]
        [SerializeField] private Transform plasticCollectorWaypoint;
        [SerializeField] private Transform woodCollectorWaypoint;
        [SerializeField] private Transform metalCollectorWaypoint;
        
        [Header("Waypoints Delivery")]
        [SerializeField] private Transform deliveryWaypoint;
        
        [Header("Detection")]
        [SerializeField, Min(1)] private float detectionRadius = 1;
        [SerializeField] private LayerMask collectorLayerMask;
        [SerializeField] private LayerMask assemblerLayerMask;
        [SerializeField] private LayerMask deliveryLayerMask;
        
        [Header("Carried Item")]
        [SerializeField] private MeshFilter carriedItemRenderer;
        
        [Header("Animations")]
        [SerializeField] private Animator animator;
        [SerializeField] private float waitTimeTakeItem = 1f;
        [SerializeField] private float waitTimeDeliverItem = 1f;
        
        private List<SO_Order> availableOrders = new List<SO_Order>();
        private NavMeshAgent navMeshAgent;
        private Transform currentAssemblerWaypoint;
        private Transform currentCollectorWaypoint;
        private SO_Order currentOrder;
        private SO_CollectableItem carriedItem;
        private AssemblerInteraction lastAssembler;
        
        private event Action OnOrderCompleted;
        public event Action<SO_Order> OnOrderStarted;
        
        private bool isMovingToCollector = false;
        private bool isMovingToAssembler = false;

        private void Awake()
        {
            if (GetComponent<NavMeshAgent>() is {} agent)
                navMeshAgent = agent;
            else Debug.LogError("PNJManager requires a NavMeshAgent component.");
            
            if (!animator) Debug.LogError("Animator is not assigned in the inspector.");
        }

        private void Start()
        {
            OnOrderCompleted += StartManageOrder;
            
            StartManageOrder();
        }

        private void SetCurrentAssemblerWaypoint(Materials material)
        {
            currentAssemblerWaypoint = material switch
            {
                Materials.Plastic => plasticAssemblerWaypoint,
                Materials.Wood => woodAssemblerWaypoint,
                Materials.Metal => metalAssemblerWaypoint,
                _ => null
            };
        }
        
        private void SetCurrentCollectorWaypoint(Materials material)
        {
            currentCollectorWaypoint = material switch
            {
                Materials.Plastic => plasticCollectorWaypoint,
                Materials.Wood => woodCollectorWaypoint,
                Materials.Metal => metalCollectorWaypoint,
                _ => null
            };
        }
        
        private void SetRandomOrder()
        {
            if (availableOrders.Count == 0)
                availableOrders = new List<SO_Order>(orders);
            
            int index = Random.Range(0, availableOrders.Count);
            currentOrder = availableOrders[index];
            currentOrder.InitDeliveryStatus();
            
            availableOrders.RemoveAt(index);
            
            OnOrderStarted?.Invoke(currentOrder);
        }

        private IEnumerator ManageOrder()
        {
            SetRandomOrder();
            SetCurrentAssemblerWaypoint(currentOrder.mainMaterial);

            foreach (var material in currentOrder.Materials)
                for (int i = 0; i < material.amount; i++)
                    yield return DeliverMaterial(material.item);
            
            if (currentOrder.IsOrderComplete())
            {
                yield return SimulateCraft();
                ChangeCarriedItem(currentOrder.finalItem);
                
                yield return GoToTarget(deliveryWaypoint.position);

                foreach (var deliveryCollider in Physics.OverlapSphere(navMeshAgent.transform.position, detectionRadius, deliveryLayerMask))
                {
                    if (deliveryCollider.GetComponent<DeliveryPointManagement>() is not { } deliveryPoint) continue;
                    if (!deliveryPoint.CanDeliver(carriedItem)) continue;
                    
                    ChangeCarriedItem(null);
                    
                    OnOrderCompleted?.Invoke();
                    break;
                }
            }
            else Debug.LogWarning("Order is not complete whereas all the materials has been delivered.");
        }

        private IEnumerator DeliverMaterial(SO_CollectableItem item)
        {
            SetCurrentCollectorWaypoint(currentOrder.mainMaterial);
            
            isMovingToCollector = true;
            
            yield return GoToTarget(currentCollectorWaypoint.position);
            
            foreach (var collectorCollider in Physics.OverlapSphere(navMeshAgent.transform.position, detectionRadius, collectorLayerMask))
            {
                if (collectorCollider.GetComponent<Shelf>() is not { } shelf) continue;
                if (shelf.TakeItem() != item) continue;
                
                animator.SetTrigger("Interact");
                
                ChangeCarriedItem(item);
                break;
            }
            yield return new WaitForSeconds(waitTimeTakeItem);
            
            isMovingToCollector = false;
            isMovingToAssembler = true;
            
            yield return GoToTarget(currentAssemblerWaypoint.position);
            
            foreach (var assemblerCollider in Physics.OverlapSphere(navMeshAgent.transform.position, detectionRadius, assemblerLayerMask))
            {
                if (assemblerCollider.GetComponent<AssemblerInteraction>() is not { } assembler) continue;
                if (!assembler.TryDeliverItem(carriedItem)) continue;
                
                animator.SetTrigger("Interact");
                
                lastAssembler = assembler;
                ChangeCarriedItem(null);
                break;
            }
            yield return new WaitForSeconds(waitTimeDeliverItem);
            isMovingToAssembler = false;
        }

        private IEnumerator GoToTarget(Vector3 target)
        {
            navMeshAgent.SetDestination(target);
            yield return new WaitWhile(() => navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance);
        }

        private IEnumerator SimulateCraft()
        {
            if (lastAssembler.GetComponent<CraftAnim>() is { } craftAnim)
            {
                craftAnim.PlayParticleSystem();
                yield return new WaitForSeconds(craftAnim.GetCraftDuration());
            }
            else Debug.LogWarning("No CraftAnim component found on the last assembler.");
        }

        private void StartManageOrder()
        {
            if (orders.Count == 0)
            {
                Debug.LogWarning("No orders available for PNJManager.");
                return;
            }
            
            StartCoroutine(ManageOrder());
        }

        private void ChangeCarriedItem(SO_CollectableItem item)
        {
            carriedItem = item;
            carriedItemRenderer.mesh = carriedItem?.itemMesh;
            carriedItemRenderer.gameObject.SetActive(carriedItem);
            animator.SetBool("IsCarryingObject", carriedItem);
        }

        private void Update()
        {
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
            animator.SetBool("IsCarryingObject", carriedItem);
        }
    }
}