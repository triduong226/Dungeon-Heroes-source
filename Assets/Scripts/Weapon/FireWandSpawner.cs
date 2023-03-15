using System.Collections;
using UnityEngine;

public class FireWandSpawner : WeaponSpawner
{
    int effectNum = 3;
    const float spreadAngle = 15f;
    const float speed = 200f;
    const float delay = 0.07f;

    protected override IEnumerator StartAttack()
    {
        EnemySpawner enemySpawner = EnemySpawner.GetInstance();

        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();

            if (enemySpawner.GetListCount() > 0)
            {
                // ��ǥ���� ���� ����
                Vector2 destination = (enemySpawner.GetRandomEnemyPosition() - (Vector2)transform.position).normalized;
                float newSpreadAngle = 0f;

                for (int i = 0; i < effectNum; ++i)
                {
                    if (i % 2 == 1)
                        newSpreadAngle += spreadAngle;

                    SpawnWeapon(newSpreadAngle, destination);

                    yield return new WaitForSeconds(delay);

                    newSpreadAngle *= -1;
                }
            }
            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    void SpawnWeapon(float spreadAngle, Vector2 destination)
    {
        GameObject weapon = ObjectPooling.GetObject(GetWeaponType());
        float destLength = (destination - (Vector2)transform.position).magnitude;
        Vector2 destVector;
        float angle;

        weapon.transform.position = GetWeaponData().GetBasePosition();

        if (GetWeaponData().GetParent().Equals(WeaponData.Parent.Self))
            weapon.transform.position += Player.GetInstance().GetPosition();

        weapon.transform.localScale = new Vector2(GetWeaponData().GetBaseScale().x * (GetAdditionalScale() / 100f), GetWeaponData().GetBaseScale().y * (GetAdditionalScale() / 100f));
        weapon.GetComponent<Weapon>().SetParameters(GetWeaponData(), GetAttackPower(), GetInactiveDelay(), Direction.Self);

        // ���� ������ �߻��ϱ� ���� ���� ���� : ȸ�� ���
        if (spreadAngle != 0f)
        {
            destination.x = destination.x * Mathf.Cos(spreadAngle / 180f * Mathf.PI) - destination.y * Mathf.Sin(spreadAngle / 180f * Mathf.PI);
            destination.y = destination.x * Mathf.Sin(spreadAngle / 180f * Mathf.PI) + destination.y * Mathf.Cos(spreadAngle / 180f * Mathf.PI);
        }

        // ��������
        destVector = destination.normalized;

        // ����Ʈ ȸ�� �� ����
        if (destVector.y < 0)
            angle = -Vector2.Angle(destVector, new Vector2(1, 0));
        else
            angle = Vector2.Angle(destVector, new Vector2(1, 0));

        // ����Ʈ ȸ��
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle - 8.5f);

        weapon.SetActive(true);

        // �߻�
        weapon.GetComponent<Rigidbody2D>().AddForce(destVector * speed, ForceMode2D.Force);
    }

    public override void LevelUp()
    {
        switch (GetLevel())
        {
            case 2:
                IncreaseAttackPower(10);
                break;
            case 3:
                DecreaseAttackSpeed(10f);
                break;
            case 4:
                IncreaseAdditionalScale(10f);
                effectNum++;
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(10);
                break;
            case 7:
                effectNum++;
                break;
        }
    }
}