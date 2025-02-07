using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ICI
{
    public class Character : TurnObserver, IApplyPos<Character>
    {
        public Pos pos;

        public GameObject instance;

        public int hp;

        public int level;

        public ISkill skill;


        public int speed { get; set; }
        public float turnPercent { get; set; }

        public Character(Pos pos, int hp, int speed, int level)
        {
            this.pos = pos;
            this.hp = hp;

            this.speed = speed;
            this.turnPercent = 0;


            SpeedCounter.Instance.addCounterListener(this);
            CharactersMap.Instance.addCharacter(this);
            this.level = level;
        }

        public Character setInstance(string name)
        {
            GameObject resource = ResourcesManager.GetResources(eResourcesPath.Prefabs,name);

            instance = GameObject.Instantiate(resource ,Vector2.zero, Quaternion.identity);
            return this;
        }

        public Character applyInstancePos()
        {
            instance.transform.position = new Vector3(pos.x ,0 ,pos.z);
            return this;
        }

        public void attacked (int damage)
        {
            this.hp -= damage;
            if(this.hp <= 0 ) dead();
        }

        public void dead()
        {
            SpeedCounter.Instance.removeCounterListener(this);
            CharactersMap.Instance.removeCharacter(this);
            GameObject.Destroy(instance);
        }

        virtual public void action_Skill1()
        {
            Projectile projectile = new Projectile(this.pos, Pos.Front(), "Sphere");
            projectile.speed = 10;
            projectile.setTarget("Enemy");
            projectile.setAttack(1, new Condition());

            SpeedCounter.Instance.finishAction();

            PopupManager.ClosePopup<SkillPopup>();
        }
        virtual public void action_Skill2()
        {
            PopupManager.ClosePopup<SkillPopup>();
        }
        virtual public void action_Skill3()
        {
            PopupManager.ClosePopup<SkillPopup>();
        }



        public void action()
        {
            InputPopup.OpenPopup();
            InputPopup.setCharacter(this);
        }

        public void startOwnTurn()
        {
            instance.transform.localScale = Vector3.one * 1.2f;
        }

        public void endOwnTurn()
        {
            instance.transform.localScale = Vector3.one;
        }
    }
}
