# Motakim
A 2D Game Engine ( using MonoGame's Framework )

Motakim (or M7K) has very basic 2D game engine features, (through ECS, GUI, Input system ... etc) for now. \
Motakim is expected to be lightweight and fast in performance.

In the future there will be :
* Enhanced GUI & Menus.
* Enhanced Input system.
* An Exclusive Tile mapping system.
* An Easy-to-use save system.
* Arabic/Persian string correction support.\
\
and the best of all:
* A Powerful **Integrated Development Environment**. \
\
And much more...

## How to install

First thing you need to do is to setup MonoGame project with the platform you prefer to use.

[Visual Studio](https://docs.monogame.net/articles/getting_started/2_creating_a_new_project_vs.html)\
[Visual Studio for Mac](https://docs.monogame.net/articles/getting_started/2_creating_a_new_project_vsm.html)\
[.NET CLI (VSCode)](https://docs.monogame.net/articles/getting_started/2_creating_a_new_project_netcore.html)

 You might also want to change project framework to .NET 6.

Once you setup the project, you need to add the `Motakim.Library` package to the project.

You can add it directly in project file
```xml
<ItemGroup>
    <PackageReference Include="Motakim.Library" Version="0.*"/>
</ItemGroup>
```
Or using dotnet CLI
```
dotnet add Motakim.Library
```
Once restored, you are ready to go.

Note: if you want the Assets files to be copied automatically to your output folder, Include this to your project file.
```xml
<ItemGroup>
    <Content Include="Assets/**/*.*">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
</ItemGroup>
```

## How to use

To get started, after you setup C# monogame project.
You will not need the `Game1.cs`, but instead a game manager which will receive special core events.\
Add a C# class (`Core.cs`) that inherit a class called `GameManager`

```cs
class Core : GameManager
{
    
}
```
Now most of the game engines need a scene atleast to work. The same thing as before, you got to add a class (`InitScene.cs`) that inherits `Scene`.
```cs
class InitScene : Scene
{

}
```
Now to create entities or let's say change scene's background, Override the `Load()` Method that will be called when scene is about to load, inside the class include:  
```cs
protected override void Load()
{
    Background = Color.CornflowerBlue;
}
```
To add the scene, inside the `Core` class add:
```cs
public Core()
{
    Scenes.Add(new InitScene());
}
```
Finally you can remove `Game1.cs`, you will get an error in `Program.cs`, inside the file you will find
```cs
using (var game = new Game1())
{
    game.Run();
}
```  
Let's replace it to
```cs
Motakim.Game.Run(new Core());
```
And there you have it.

There isn't any documentation in the current time.\
~~But you can check out the **Sample** from source code.~~