using System;
using System.Collections.Generic;

namespace SimpleMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create the main menu, a simple list with 10 items
            List<string> mainMenu = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                mainMenu.Add("Menu " + i);
            }

            string path = "";
            int menuIndex = 0;
            while (menuIndex >= 0)
            {
                //Display the menu and return the selected index
                menuIndex = DisplayMenu(mainMenu.ToArray(), menuIndex);

                //Store the path to display on the sub menus
                path = AddToPath(path, menuIndex);

                while (path != "" && menuIndex >= 0)
                {
                    //Create a new sub menu
                    List<string> subMenu = new List<string>();
                    for (int i = 1; i <= 10; i++)
                    {
                        subMenu.Add("Submenu " + path + i);
                    }

                    //Display the submenu and get the selected index)
                    menuIndex = DisplayMenu(subMenu.ToArray(), 0);

                    //Add or remove the menuIndex to the path
                    if (menuIndex >= 0) path = AddToPath(path, menuIndex);
                    else path = RemoveFromPath(path);

                    //Reset the menuIndex to 0 so 1) the menus starts on top or 2) the application does not exit (yet(
                    menuIndex = 0;
                }

            }

            //We're done here
            Environment.Exit(0);
        }

        static string AddToPath(string path, int index)
        {
            return path + (index + 1) + ".";
        }

        static string RemoveFromPath(string path)
        {
            if (path == "") return "";
            return path.Substring(0, path.Length - 2);
        }

        static int DisplayMenu(string[] menuItems, int selectedIndex = 0)
        {
            Console.CursorVisible = false;
            Console.Clear();

            //Add menu items from the string
            List<MenuItem> menu = new List<MenuItem>();
            foreach (string m in menuItems)
            {
                menu.Add(new MenuItem(m));
            }
            menu[selectedIndex].Selected = true;

            while (true) //Always display the menu
            {
                int c = -1;
                foreach (MenuItem m in menu)
                {
                    c++;
                    Console.SetCursorPosition(1, c);
                    if (m.Selected)
                    {
                        //Find the index of the selected item
                        selectedIndex = menu.FindIndex(t => t.Title == m.Title);

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write(m.Title);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(m.Title);
                    }
                }

                //Weird glitch when you take this out
                Console.SetCursorPosition(1, menu.Count);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("");


                //Get the pressed key
                var inp = Console.ReadKey().Key;

                //Move up or down the menu if pressing up or down menu
                if (inp == ConsoleKey.UpArrow || inp == ConsoleKey.DownArrow)
                {
                    //Deselect the current item
                    menu[selectedIndex].Selected = false;

                    //Get the next index (lower or higher)
                    if (inp == ConsoleKey.UpArrow && selectedIndex > 0) selectedIndex -= 1;
                    else if (inp == ConsoleKey.DownArrow && selectedIndex < menu.Count - 1) selectedIndex += 1;

                    //Select the item with the new index
                    menu[selectedIndex].Selected = true;
                }

                //Return the selected index if Enter is pressed
                if (inp == ConsoleKey.Enter) return selectedIndex;

                //Backspace goes back one step (or quit at the main menu)
                if (inp == ConsoleKey.Backspace) return -1;

                //Escape quits
                if (inp == ConsoleKey.Escape) Environment.Exit(0);
            }
        }

        public class MenuItem
        {
            public string Title;
            public bool Selected;

            public MenuItem(string title)
            {
                this.Title = title;
                this.Selected = false;
            }
            public MenuItem(string title, bool selected)
            {
                this.Title = title;
                this.Selected = selected;
            }
        }
}
