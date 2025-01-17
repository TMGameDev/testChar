using UnityEngine;

public class AttachWeapon : MonoBehaviour
{
    public Transform weaponAttachPoint; // Référence au point d'attache
    private GameObject currentWeapon;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // Supprime l'arme actuelle s'il y en a une
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instancie la nouvelle arme et l'attache au point d'attache
        currentWeapon = Instantiate(weaponPrefab, weaponAttachPoint);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }
}
