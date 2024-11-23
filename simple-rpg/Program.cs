using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.VisualBasic;

internal partial class Program
{
    public static Random rng = new Random();

    /*private static void debug(Monster a, Player b)
    {
        //while (true)
        {
            //Monster a = new Monster("vampire", 1);
            //Player b = new Player("monk");
            //a.inventory[0] = 2;
            foreach (string i in a.readInventory())
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(a.health);
            Console.WriteLine(a.attack);
            Console.WriteLine(a.defense);
            Console.WriteLine(calculateDamage(b, a));
            Console.WriteLine("hello world!");
        }
    }*/

    private static Player createPlayer()
    {
        string input;
        Console.WriteLine("Choose a player class.\n1) warrior\n2) monk\n3) theif");
        Console.Write(">");
        input = Console.ReadLine();
        if (!Player.stats.ContainsKey(input))
        {
            Console.WriteLine("invalid class\ntry again\n");
            return createPlayer();
        }
        Player t = new Player(input);
        for (int a = 0; a < 4; a++)
        {
            t.addToInventory(rng.Next(5));
        }
        return t;
    }

    private static void Main(string[] args)
    {
        string input, inv;
        int damage, exp, item;
        Console.WriteLine("HEllo. Welcome to my game\n");
        string[] mtypes = { "goblin", "vampire", "dragon" };
        Player p = createPlayer();
        while (true)
        {
            Monster m = new Monster(mtypes[rng.Next(3)], p.level - 1 + rng.Next(3));
            exp = m.getExp();
            //debug(m, p);
            Console.WriteLine("you encountered a " + m.type);
            //Console.Write("\n");
            while (m.health >= 0)
            {
                Console.WriteLine("\nplayer health: " + p.health + "\t\t\t" + m.type + " health: " + m.health);
                Console.WriteLine("\n1) attack\n2) use item\n3) run");
                Console.Write(">");

                input = Console.ReadLine();
                //parseinput(input);
                Console.Write("\n");

                if (input == "attack")
                {
                    damage = calculateDamage(p, m);
                    Console.WriteLine(m.type + " took " + damage + " damage!");
                }
                else if (input == "use item")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Console.WriteLine((i + 1) + ") " + p.readInventory()[i]);
                    }
                    Console.WriteLine("which would you like to use? (1/2/3/4)");
                    Console.Write(">");

                    item = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    string tempitem = p.readInventory()[item - 1];
                    p.removeFromInventory(tempitem);

                    if (tempitem == "potion")
                    {
                        p.health += 50;
                        Console.Write("your health increased by ");
                        Console.WriteLine("50");
                    }
                    else if (tempitem == "large potion")
                    {
                        p.health += 100;
                        Console.Write("your health increased by ");
                        Console.WriteLine("100");
                    }
                    else if (tempitem == "magic berry")
                    {
                        p.health += 200;
                        Console.Write("your health increased by ");
                        Console.WriteLine("200");
                    }
                    else
                    {
                        Console.WriteLine("the monster took " + (m.health + 2) + " damage!");
                        m.health = -2;
                    }


                }
                else
                {
                    int canRunAway = rng.Next(256);
                    if (canRunAway < 220)
                    {
                        Console.WriteLine("sucessfully ran away!");
                        p.health += 50;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("couldn't run away!");
                    }
                }

                if (m.health <= 0)
                {
                    Console.WriteLine("the monster died!");
                    Console.WriteLine("you got " + exp + " exp");
                    p.addexp(exp);
                }
                else
                {
                    Console.Write("\n");
                    int monsterchoice = rng.Next(5);
                    if (monsterchoice != 0)
                    {
                        damage = calculateDamage(m, p);
                        Console.WriteLine(m.type + " attacked you!\nyou took " + damage + " points of damage!");
                    }
                    else
                    {
                        string tempitem = "empty";
                        int flag = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (m.readInventory()[i] != "empty")
                            {
                                tempitem = m.readInventory()[i];
                                flag = 1;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            Console.WriteLine("the monster did nothing");
                        }
                        else
                        {
                            m.removeFromInventory(tempitem);
                            Console.Write("the monster used " + tempitem + "!");
                            if (tempitem == "potion")
                            {
                                m.health += 50;
                                Console.Write("its health increased by ");
                                Console.WriteLine("50\n");
                            }
                            else if (tempitem == "large potion")
                            {
                                m.health += 100;
                                Console.Write("its health increased by ");
                                Console.WriteLine("100\n");
                            }
                            else if (tempitem == "magic berry")
                            {
                                m.health += 200;
                                Console.Write("its health increased by ");
                                Console.WriteLine("200");
                            }
                            else
                            {
                                Console.WriteLine("you took " + (p.health + 2) + " damage!");
                                p.health = -2;
                            }
                        }
                    }
                }

                if (p.health <= 0)
                {
                    Console.WriteLine("You died...");
                    while (true)
                    {
                        Thread.Sleep(10000);
                        Console.ReadKey(false);
                    }
                    return;
                }

                p.health += 50;

            }
        }
        //debug();
    }

    private class Actor
    {
        public int health = 0, attack = 0, defense = 0;
        // make inventory private later
        private int[] inventory = { 0, 0, 0, 0 };
        public Dictionary<int, String> items = new Dictionary<int, String>() {
            { 0, "empty"},
            {1, "potion" },
            {2, "large potion" },
            {3, "magic berry" },
            {4, "fireball" }
        };

        public Dictionary<String, int> itemcodes = new Dictionary<String, int>() {
               {"empty", 0},
               {"potion", 1 },
               {"large potion", 2},
               {"magic berry", 3},
               {"fireball", 4 }
        };
        
        public String[] readInventory()
        {
            String[] result = new String[this.inventory.Length];
            for (int i = 0; i < this.inventory.Length; i++)
            {
                result[i] = items[this.inventory[i]];
            }
            return result;
        }

        public void addToInventory(int itemToAdd) {
            bool flag = true;
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (this.inventory[i] == 0){
                    this.inventory[i] = itemToAdd;
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("not enough space in inventory");
            }
        }

        public void removeFromInventory(string item)
        {
            bool flag = true;
            int itemToRemove = itemcodes[item];
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (this.inventory[i] == itemToRemove)
                {
                    this.inventory[i] = 0;
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine("item not in inventory");
            }
        }
    }

    private class Monster : Actor
    {
        public string type;
        Dictionary<string, (int, int, int)> stats = new Dictionary<string, (int, int, int)>
        {
            {"goblin", (100, 88, 196) },
            {"vampire", (150, 250, 200)},
            {"dragon", (255, 255, 255) }
        };

        public Monster(string type, int level)
        {
            int ivs = rng.Next(85, 115);
            this.type = type;
            (this.health, this.attack, this.defense) = stats[type];
            this.health = (int)((double)(this.health * level) * (double)ivs / 100.0) + 2;
            this.attack = (int)((double)(this.attack * level) * (double)ivs / 100.0) + 2;
            this.defense = (int)((double)(this.defense * level) * (double)ivs / 100.0) + 2;
            this.addToInventory(rng.Next(5));
        }

        public int getExp()
        {
            return this.health + this.attack + this.defense;
        }
    }

    private class Player : Actor
    {
        public string type;
        public int level = 1, exp = 0, nextexp = 1;
        public static Dictionary<string, (int, int, int)> stats = new Dictionary<string, (int, int, int)>
        {
            {"warrior", (255, 255, 255) },
            {"monk", (226, 339, 200)},
            {"theif", (265, 218, 282) }
        };

        public Player(string type)
        {
            int ivs = rng.Next(95, 125);
            this.type = type;
            (this.health, this.attack, this.defense) = stats[type];
            this.health = (int)((double)(this.health * level) * (double)ivs / 100.0) + 2;
            this.attack = (int)((double)(this.attack * level) * (double)ivs / 100.0) + 2;
            this.defense = (int)((double)(this.defense * level) * (double)ivs / 100.0) + 2;
        }

        void levelup()
        {
            while (this.nextexp <= 0)
            {
                this.health /= this.level;
                this.attack /= this.level;
                this.defense /= this.level;
                this.level += 1;
                this.nextexp = this.level * this.level * this.level - this.exp;
                this.health *= this.level;
                this.attack *= this.level;
                this.defense *= this.level;
            }
        }

        public void addexp(int xp)
        {
            this.exp += xp;
            this.nextexp -= xp;
            if (this.nextexp <= 0)
            {
                this.levelup();
            }
        }
    }

    private static int calculateDamage(Actor attacker, Actor defender)
    {
        double temp = (attacker.attack - defender.defense) * ((double)rng.Next(232, 256)/255.0);
        temp /= 1.2;
       if (temp < 0)
        {
            temp *= (-1) * ((double)attacker.attack / (double)defender.defense);
        }

        int damage = (int)temp + 2;
        defender.health -= damage;
        return damage;
    }

}