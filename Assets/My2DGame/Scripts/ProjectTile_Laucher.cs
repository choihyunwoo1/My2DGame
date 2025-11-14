using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 발사체를 발사하는 스크립트
    /// </summary>
    public class ProjectTile_Laucher : MonoBehaviour
    {
        #region Variables
        public GameObject projectTilePrefab; //Bullet
        public Transform firePoint; //FirePoint
        #endregion

        #region Uinty Event Method

        #endregion

        #region Custom Method
        public void FireProjectTile()
        {   
            Instantiate(projectTilePrefab, firePoint.position, Quaternion.identity);
        }
        #endregion 
    }
}