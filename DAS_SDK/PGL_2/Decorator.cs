using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.PGL_2
{
    abstract class Decorator : Animal
    {
        protected Animal _animal;
        protected float x;

        public void SetAnimal(Animal animal)
        {
            this._animal = animal;
        }
    }

    abstract class Decorator2 : Animal
    {
        protected Animal _animal;
        protected RunningAnimal _Ranimal;
        protected float z;

        public void SetAnimal(Animal animal)
        {
            RunningAnimal rnAnim = new RunningAnimal();
            if ((animal as RunningAnimal) == null)
            {
                rnAnim.SetAnimal(animal);
                _Ranimal.SetAnimal(rnAnim);
            }
            else
            {
                this._Ranimal = animal as RunningAnimal;
                _Ranimal.SetAnimal(_Ranimal);
            }
           
        }
    }

    abstract class Animal
    {
        public string voiceLine = "no_null_can_speak";
        public abstract void Make_a_Sound();
    }

    class Dog : Animal
    {
        public Dog(string voiceLine)
        {
            this.voiceLine = voiceLine;
        }

        public override void Make_a_Sound()
        {
            Console.WriteLine(voiceLine);
        }
    }

    class Cat : Animal
    {
        public Cat(string voiceLine)
        {
            this.voiceLine = voiceLine;
        }

        public override void Make_a_Sound()
        {
            Console.WriteLine(voiceLine);
        }
    }

    class RunningAnimal : Decorator
    {
        public override void Make_a_Sound()
        {
            Console.WriteLine(_animal.voiceLine + " => Type: [" + this.GetType().Name + "] [" + _animal.GetType().Name + "]"); 
        }

        public void Run()
        {
            x += 20.2f;
            Console.WriteLine("X: "  + this.x);
        } 
    }

    class FlyingAnimal : Decorator2
    {
        public RunningAnimal GetRunningAnimal()
        {
            return this._Ranimal;
        }

        public override void Make_a_Sound()
        {
            Console.WriteLine(_Ranimal.voiceLine + " => Type: [" + this.GetType().Name + "] [" + _Ranimal.GetType().Name + "]");
        }

        public void Fly()
        {
            z += 20.2f;
            Console.WriteLine("Z: " + this.z);
        }
    }


}
