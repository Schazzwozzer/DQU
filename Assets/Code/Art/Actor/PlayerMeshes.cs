using System.Collections.Generic;
using UnityEngine;

namespace DQU.Art
{

    public class PlayerMeshes : MonoBehaviour
    {
        // The goal of this component is to allow multiple meshes
        // to use one single skeleton rig for their animation.


        [SerializeField]
        private Transform _rig;

        [SerializeField]
        private Transform _rigRootBone;

        [SerializeField]
        private GameObject _bodyPrefab;

        [SerializeField]
        private GameObject _clothingPrefab;

        /// <summary>
        /// The bones of this component's rig skeleton, acccessible by name.
        /// </summary>
        private Dictionary<string, Transform> _bonesByName;


        private void Start()
        {
            _bonesByName = CollateDictionary();

            if( _bodyPrefab )
            {
                SkinnedMeshRenderer bodyMesh = GameObject.Instantiate(
                    _bodyPrefab.GetComponentInChildren<SkinnedMeshRenderer>().gameObject, Vector3.zero, Quaternion.identity, this.transform ).GetComponent<SkinnedMeshRenderer>();
                SwitchToNewSkeleton( bodyMesh, _rig );
            }

            if( _clothingPrefab )
            {
                SkinnedMeshRenderer armorMesh = GameObject.Instantiate<SkinnedMeshRenderer>(
                    _clothingPrefab.GetComponentInChildren<SkinnedMeshRenderer>(), Vector3.zero, Quaternion.identity, this.transform );
                SwitchToNewSkeleton( armorMesh, _rig );
            }
            //_rig.localRotation = Quaternion.identity;
        }


        /// <summary>
        /// Create an dictionary of all of the bones in the target Skeleton, easily accessible by their GameObject's name.
        /// </summary>
        private Dictionary<string, Transform> CollateDictionary()
        {
            
            Dictionary<string, Transform> bonesByName = new Dictionary<string, Transform>();

            AddChildrenToDictionary( bonesByName, _rig );

            return bonesByName;
        }

        /// <summary>
        /// Recursively add the specified <paramref name="parent"/> bone
        /// to the dictionary, as well as all of the bone's child bones.
        /// </summary>
        /// <param name="parent">The bone to be added to the dictionary.</param>
        private void AddChildrenToDictionary( Dictionary<string, Transform> dict, Transform parent )
        {
            for( int i = 0; i < parent.childCount; ++i )
            {
                Transform child = parent.GetChild( i );
                dict[child.name] = child;

                if( child.childCount > 0 )
                    AddChildrenToDictionary( dict, child );
            }
        }

        /// <summary>
        /// Replace all of the mesh's bones with those of this component's target rig.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="bonesByName"></param>
        /// <param name="skeleton"></param>
        private void SwitchToNewSkeleton( 
            SkinnedMeshRenderer mesh, 
            Transform skeleton )
        {
            Transform[] newBones = new Transform[mesh.bones.Length];

            mesh.rootBone = _rigRootBone;

            for( int i = 0; i < mesh.bones.Length; ++i )
            {
                Transform newBone;
                if( _bonesByName.TryGetValue( mesh.bones[i].name, out newBone ) )
                    newBones[i] = newBone;
                else
                    Debug.LogError(
                        string.Format(
                            "Mesh {0} can not find bone \"{1}\"", gameObject.name, mesh.bones[i].name ),
                        mesh.gameObject );
            }

            mesh.bones = newBones;
        }

    }
}