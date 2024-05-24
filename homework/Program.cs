using System;

namespace ArenaGameEngine
{
    public abstract class Hero
    {
        public string Name { get; }
        public int Health { get; protected set; }
        public int AttackPower { get; protected set; }
        public bool IsAlive => Health > 0;

        protected Hero(string name)
        {
            Name = name;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        public abstract void Attack(Hero target);
    }

    public class Arena
    {
        private readonly Hero _hero1;
        private readonly Hero _hero2;
        public GameEventListener EventListener { get; set; }

        public Arena(Hero hero1, Hero hero2)
        {
            _hero1 = hero1;
            _hero2 = hero2;
        }

        public Hero Battle()
        {
            Hero attacker = _hero1;
            Hero defender = _hero2;
            while (attacker.IsAlive && defender.IsAlive)
            {
                int attack = attacker.AttackPower;
                defender.TakeDamage(attack);
                EventListener?.GameRound(attacker, defender, attack);
                (attacker, defender) = (defender, attacker);
            }

            return attacker.IsAlive ? attacker : defender;
        }
    }

    public abstract class GameEventListener
    {
        public abstract void GameRound(Hero attacker, Hero defender, int attack);
    }
}

namespace ArenaGameConsole
{
    using ArenaGameEngine;

    class ConsoleGameEventListener : GameEventListener
    {
        public override void GameRound(Hero attacker, Hero defender, int attack)
        {
            string message = $"{attacker.Name} attacked {defender.Name} for {attack} points";
            if (defender.IsAlive)
            {
                message += $" but {defender.Name} survived.";
            }
            else
            {
                message += $" and {defender.Name} died.";
            }
            Console.WriteLine(message);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Mage mage = new Mage("Elsa");
            Berserker berserker = new Berserker("Olaf");

            Arena arena = new Arena(mage, berserker);
            arena.EventListener = new ConsoleGameEventListener();

            Console.WriteLine("Battle begins.");
            Hero winner = arena.Battle();
            Console.WriteLine($"Battle ended. Winner is: {winner.Name}");
            Console.ReadLine();
        }
    }

    public class Mage : Hero
    {
        public Mage(string name) : base(name)
        {
            Health = 80;
            AttackPower = 30;
        }

        public override void Attack(Hero target)
        {
            target.TakeDamage(AttackPower);
        }
    }

    public class Berserker : Hero
    {
        public Berserker(string name) : base(name)
        {
            Health = 100;
            AttackPower = 20;
        }

        public override void Attack(Hero target)
        {
            target.TakeDamage(AttackPower);
        }
    }
}


