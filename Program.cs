using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        AnimalMaker animalMaker = new AnimalMaker();
        Zoo zoo = new Zoo( animalMaker);
        Game game = new Game();
        game.Play(zoo);
    }
}

public class Game
{
    public void Play(Zoo zoo)
    {
        const string CommandShowAllCellsTypes = "1";
        const string CommandShowAnimalsInCell = "2";
        const string CommandShowAllCells = "3";
        const string CommandExit = "9";
        bool isRun = true;

        Console.WriteLine($"Menu: {CommandShowAllCellsTypes}-Show all cells types;");
        Console.WriteLine($"      {CommandShowAnimalsInCell}-Show animal in cell;");
        Console.WriteLine($"      {CommandShowAllCells}-Show all animals in all cells;");
        Console.WriteLine($"      {CommandExit}-Exit;");

        while (isRun)
        {
            switch (Utils.ReadString("Your shois: "))
            {
                case CommandShowAllCellsTypes:
                    zoo.ShowAllTypes();
                    break;
                case CommandShowAnimalsInCell:
                    zoo.ShowAnimalsInCell(Utils.ReadInt("Input number of cell"));
                    break;
                case CommandShowAllCells:
                    zoo.ShowAllData();
                    break;
                case CommandExit:
                    isRun = false;
                    break;
            }
        }
    }
}

public class Animal
{
    private string _sex;
    private string _sound;
    private int _age;

    public Animal(string type, string sex, string sound, int age)
    {
        Type = type;
        _sex = sex;
        _sound = sound;
        _age = age;
    }

    public string Type { get; }

    public void ShowStats() =>
        Console.WriteLine(Type + "   " + _sex + "    " + _sound + "  " + _age);
}

public class Cell
{
    private List<Animal> _animals = new List<Animal>();

    public Cell(AnimalMaker animalMaker)
    {
        _animals = animalMaker.CreateListAnimals();
    }

    public void ShowAllData()
    {
        foreach (var animal in _animals)
            animal.ShowStats();
    }

    public void ShowType() =>
        Console.WriteLine(_animals[0].Type);
}

public class Zoo
{
    private List<Cell> _cells = new List<Cell>();
    private int _quantityOfCell = 5;

    public Zoo( AnimalMaker animalMaker)
    {
        for (int i = 0; i < _quantityOfCell; i++)
            _cells.Add(new Cell(animalMaker));
    }

    public void ShowAllData()
    {
        foreach (var cell in _cells)
        {
            Console.WriteLine("#####################################");
            cell.ShowAllData();
            Console.WriteLine("#####################################");
        }
    }

    public void ShowAllTypes()
    {
        Console.WriteLine("cell" + "  " + "type");

        for (int i = 0; i < _cells.Count; i++)
        {
            Console.Write(i + "     ");
            _cells[i].ShowType();
        }
    }

    public void ShowAnimalsInCell(int numberOfCell)
    {
        if (numberOfCell > _cells.Count || numberOfCell < 0)
        {
            Console.WriteLine("Incorrect index");
            return;
        }

        _cells[numberOfCell].ShowAllData();
    }
}

public class AnimalMaker
{
    public List<Animal> CreateListAnimals()
    {
        string[,] _types = new string[,] { { "monkey", "ii-ii" },
                                               { "lion","rr-rr" },
                                               { "horse", "igo-go" },
                                               { "wolf", "gaw-gaw" },
                                               { "crocodile","ff-ff" },
                                               { "rabbit","piu-piu" } };
        string[] _sexs = new string[] { "male", "female" };
        int _maxAge = 15;
        int _minAge = 1;
        int _minQuantityAnimals = 2;
        int _maxQuantityAnimals = 12;

        int positionInArrayTypes = Utils.GenerateRandomNumber(0, _types.GetLength(0));
        string type = _types[positionInArrayTypes, 0];
        string sound = _types[positionInArrayTypes, 1];

        int quantityOfAnimals = Utils.GenerateRandomNumber(_minQuantityAnimals, _maxQuantityAnimals);

        List<Animal> tempList = new List<Animal>();

        for (int i = 0; i < quantityOfAnimals; i++)
        {
            int positionInArraySexs = Utils.GenerateRandomNumber(0, _sexs.Length);
            string sex = _sexs[positionInArraySexs];
            int age = Utils.GenerateRandomNumber(_minAge, _maxAge);

            tempList.Add(new Animal(type, sex, sound, age));
        }

        _types = Utils.DeleteElementInDoubleArray(_types, positionInArrayTypes);
        return tempList;
    }
}

public static class Utils
{
    private static Random s_random = new Random();

    public static int GenerateRandomNumber(int min, int max)
    {
        return s_random.Next(min, max);
    }

    public static string[,] DeleteElementInDoubleArray(string[,] array, int positionOfElement)
    {
        if (positionOfElement >= array.GetLength(0) || positionOfElement < 0)
        {
            Console.WriteLine("Incorrect index");
            return null;
        }

        string[,] arrayNew = new string[array.GetLength(0) - 1, array.GetLength(1)];

        int positionInArray = 0;
        int positionInNewArray = 0;

        while (positionInArray < array.GetLength(0))
        {
            if (positionInArray != positionOfElement)
            {
                for (int t = 0; t < array.GetLength(1); t++)
                    arrayNew[positionInNewArray, t] = array[positionInArray, t];

                positionInNewArray++;
            }

            positionInArray++;
        }

        return arrayNew;
    }

    public static string ReadString(string text = "")
    {
        Console.Write(text + " ");
        string stringFromConsole = Console.ReadLine();
        return stringFromConsole;
    }

    public static int ReadInt(string text = "")
    {
        int digitToOut = 0;
        bool isRun = true;

        while (isRun)
        {
            Console.Write(text + " ");
            string digitFromConsole = Console.ReadLine();

            if (int.TryParse(digitFromConsole, out digitToOut))
            {
                isRun = false;
            }
        }

        return digitToOut;
    }
}