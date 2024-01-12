<a name="table-of-contents"></a>
# Project Style Guide

This Style Guide explains the ideas behind the projects structure and should strictly be followed to ensure consistency throughout the project. However as any Style Guide it should be viewed as a living document and can be changed if it benefits all usages.

**If you see someone working either against a style guide or no style guide, try to correct them.**

### Table of Contents
> 1. [Project Structure](#structure)
> 1. [Asset Naming Convetions](#asset-names)
> 1. [Asset Workflows](#asset-workflows)
> 1. [Scripts](#scripts)
> 1. [Code Workflows](#code-workflows) 
> 1. [Glossary](#glossary)

----------------------------

<a name="structure"></a>
## 1. Project Structure
This structure is considered law. [Asset naming conventions](#asset-name-modifiers) and content directory structure go hand in hand, and a violation of either causes unneeded chaos.

This Project is using a structure that relies more on filtering and search abilities of the Project Window for those working with assets to find assets of a specific type instead of another common structure that groups asset types with folders.

> Using a prefix [naming convention](#asset-name-modifiers), using folders to contain assets of similar types such as `Meshes`, `Textures`, and `Materials` is a redundant practice as asset types are already both sorted by prefix as well as able to be filtered in the content browser.

<pre>
Assets
    <a name="structure-top-level">_Dev</a>
        DeveloperName
            (Work in progress assets)
    <a name="structure-top-level">Project</a>
        Audio
            - SFX
            - Music
        Art
        Scenes
            - Levels
            - Other
        Prefabs
        ScriptableObjects
        Scripts
            - Core
            - Tools
    <a name="structure-top-level">ThirdParty</a>
        Art
        Plugins
</pre>

### 1.1 Folder Names
These are common rules for naming any folder in the content structure.
- Always Use [PascalCase](#terms-cases).
- Never Use Spaces.
- Never Use Unicode Characters or Symbols.
### 1.2 No Empty Folders, they clutter the content browser.
There simply shouldn't be any empty folders. They clutter the content browser.

### 1.3 Keep Unfinished Stuff in the Development Folder
The `_Dev` folder is not for assets that your project relies on and therefore is not project specific.
- No Global Assets.

### 1.4 Do Not Create Folders named after Types.
Don't create the following folders:
`Assets`, `AssetTypes`, `Meshes`, `Textures`, `Materials`
ll asset names are named with their asset type in mind. These folders offer only redundant information and the use of these folders can easily be replaced with the robust and easy to use filtering system the Content Browser provides.

Want to view only static mesh in `Environment/Rocks/`? Simply turn on the Static Mesh filter. If all assets are named correctly, they will also be sorted in alphabetical order regardless of prefixes. Want to view both static meshes and skeletal meshes? Simply turn on both filters. this eliminates the need to potentially have to `Control-Click` select two folders in the Content Browser's tree view.

### 1.5 Very Large Asset Sets Get Their Own Folder Layout
There are certain asset types that belong together, they should be together.

For example, animations that are shared across multiple characters should lay in `Characters/Common/Animations` and may have sub-folders such as `Locomotion` or `Cinematic`.

> This does not apply to assets like textures and materials. It is common for a `Rocks` folder to have a large amount of textures if there are a large amount of rocks, however these textures are generally only related to a few specific rocks and should be named appropriately. Even if these textures are part of a [Material Library](#structure-material-library).
<a name="structure-material-library"></a>
### 1.6 `MaterialLibrary`
If your project makes use of master materials, layered materials, or any form of reusable materials or textures that do not belong to any subset of assets, these assets should be located in `Assets/ProjectName/MaterialLibrary`.

This way all 'global' materials have a place to live and are easily located.

> This also makes it incredibly easy to enforce a 'use material instances only' policy within a project. If all artists and assets should be using material instances, then the only regular material assets that should exist are within this folder. You can easily verify this by searching for base materials in any folder that isn't the `MaterialLibrary`.

The `MaterialLibrary` doesn't have to consist of purely materials. Shared utility textures, material functions, and other things of this nature should be stored here as well within folders that designate their intended purpose. For example, generic noise textures should be located in `MaterialLibrary/Utility`.

Any testing or debug materials should be within `MaterialLibrary/Debug`. This allows debug materials to be easily stripped from a project before shipping and makes it incredibly apparent if production assets are using them if reference errors are shown.

<a name="structure-assemblies"></a>
### 1.7 Assembly Definitions
The bigger the code base for the unity project becomes, the longer it needs to compile. To speed up compile time we can use Assemblies. Assemblies can split our code into different parts that will compile independently. 

Assemblies can be created in any asset folder by clicking `Create/Assembly Definition`. Every Script in this folder and its sub folders is now included in the assembly you created.

#### 1.7.1 Assembly Architecture
To create and maintain an efficient assembly architecture, you should create an Assembly Definition for the following:
* **ThirdParty Folder** - One Assembly Definition to seperate all Third Party code from the projects code.
* **Sub Folders of Script Folder** - Every sub folder of the projects script folder should work independently and compile on it's own. If one folder requires to use code of another one, you can define a Reference to another.

> Assembly Definitions should mirror your [name spaces](#namespace).

For a detailed explanation watch [this video](https://www.youtube.com/watch?v=eovjb5xn8y0) by Game Dev Guide.


## 1.8 Scene Structure
Next to the project’s hierarchy, there’s also scene hierarchy. As before, we’ll present you a template. You can adjust it to your needs. Use named empty game objects as scene folders.

<pre>
DEBUG
    GUI
MANAGEMENT
    @Pooler
    @Audio
CAMERAS
    Brain
    Other
WORLD
    Lighting
    Environment
    Props
GAMEPLAY
    Player
    Enemies
    Items
_DYNAMIC
</pre>

 - All empty objects should be located at 0,0,0 with default rotation and scale.
 - For empty objects that are only containers for scripts, use “@” as prefix – e.g. @Cheats
 - When you’re instantiating an object in runtime, make sure to put it in _Dynamic – do not pollute the root of your hierarchy or you will find it difficult to navigate through it.
 - Objects that wont move position like Managers should be marked as static in the Inspector.

**[⬆ Back to Top](#table-of-contents)**

----------------------------
<a name="asset-naming"></a>
## 2. Asset Naming Conventions
Naming conventions should be treated as law. A project that conforms to a naming convention is able to have its assets managed, searched, parsed, and maintained with incredible ease.

Most things are prefixed with the prefix generally being an acronym of the asset type followed by an underscore.

**Assets use [PascalCase](#cases)**

<a name="base-asset-name"></a>
<a name="asset-naming-base"></a>
### 2.1 Base Asset Name 
`Prefix_BaseAssetName_Variant_Suffix`
All assets should have a _Base Asset Name_. A Base Asset Name represents a logical grouping of related assets. Any asset that is part of this logical group 
should follow the the standard of  `Prefix_BaseAssetName_Variant_Suffix`.

Keeping the pattern `Prefix_BaseAssetName_Variant_Suffix` in mind and using common sense is generally enough to warrant good asset names. Here are some detailed rules regarding each element.

`Prefix` and `Suffix` are to be determined by the asset type through the following [Asset Name Modifier](#asset-name-modifiers) tables.

`BaseAssetName` should be determined by short and easily recognizable name related to the context of this group of assets. For example, if you had a character named Bob, all of Bob's assets would have the `BaseAssetName` of `Bob`.

For unique and specific variations of assets, `Variant` is either a short and easily recognizable name that represents logical grouping of assets that are a subset of an asset's base name. For example, if Bob had multiple skins these skins should still use `Bob` as the `BaseAssetName` but include a recognizable `Variant`. An 'Evil' skin would be referred to as `Bob_Evil` and a 'Retro' skin would be referred to as `Bob_Retro`.

For unique but generic variations of assets, `Variant` is a two digit number starting at `01`. For example, if you have an environment artist generating nondescript rocks, they would be named `Rock_01`, `Rock_02`, `Rock_03`, etc. Except for rare exceptions, you should never require a three digit variant number. If you have more than 100 assets, you should consider organizing them with different base names or using multiple variant names.

Depending on how your asset variants are made, you can chain together variant names. For example, if you are creating flooring assets for an Arch Viz project you should use the base name `Flooring` with chained variants such as `Flooring_Marble_01`, `Flooring_Maple_01`, `Flooring_Tile_Squares_01`.

<a name="asset-naming-modifiers"></a>
### 2.2 Asset Name Modifiers
When naming an asset use these tables to determine the prefix and suffix to use with an asset's [Base Asset Name](#base-asset-name).

#### Sections

> 2.2.1 [Most Common](#anc-common)

> 2.2.2 [Animations](#anc-animations)

> 2.2.3 [Artificial Intelligence](#anc-ai)

> 2.2.4 [Prefabs](#anc-prefab)

> 2.2.5 [Materials](#anc-materials)

> 2.2.6 [Textures](#anc-textures)

> 2.2.7 [Miscellaneous](#anc-misc)

> 2.2.8 [Physics](#anc-physics)

> 2.2.9 [Audio](#anc-audio)

> 2.2.10 [User Interface](#anc-ui)

> 2.2.11 [Effects](#anc-effects)

<a name="anc-common"></a>
#### 2.2.1 Most Common

| Asset Type              | Prefix     | Suffix     | Notes                            |
| ----------------------- | ---------- | ---------- | -------------------------------- |
| Level / Scene           |  *         |            | [Should be in a folder called Levels.](#levels) e.g. `Levels/A4_C17_Parking_Garage.unity` |
| Level (Persistent)      |            | _P         |                                  |
| Level (Audio)           |            | _Audio     |                                  |
| Level (Lighting)        |            | _Lighting  |                                  |
| Level (Geometry)        |            | _Geo       |                                  |
| Level (Gameplay)        |            | _Gameplay  |                                  |
| Prefab                  |            |            |                                  |
| Probe (Reflection)      | RP_        |            |                                  |
| Probe (Light)           | LP_        |            |                                  |
| Volume                  | V_         |            |                                  |
| Trigger Area            |            | _Trigger   |                                  |
| Material                | M_         |            |                                  |
| Static Mesh             | SM_        |            |                                  |
| Skeletal Mesh           | SK_        |            |                                  |
| Texture                 | T_         | _?         | See [Textures](#anc-textures)    |
| Visual Effects          | VFX_       |            |                                  |
| Particle System         | PS_        |            |                                  |
| Light                   | L_         |            |                                  |
| Camera (Cinemachine)    | CM_        |            | Virtual Camera                   |

<a name="anc-models"></a>

#### 2.2.1a 3D Models (FBX Files)

PascalCase

| Asset Type    | Prefix | Suffix | Notes |
| ------------- | ------ | ------ | ----- |
| Characters    | CH_    |        |       |
| Vehicles      | VH_    |        |       |
| Weapons       | WP_    |        |       |
| Static Mesh   | SM_    |        |       |
| Skeletal Mesh | SK_    |        |       |
| Skeleton      | SKEL_  |        |       |
| Rig           | RIG_   |        |       |

#### 2.2.1b 3d Models (3ds Max)

All meshes in 3ds Max are lowercase to differentiate them from their FBX export.

| Asset Type    | Prefix | Suffix      | Notes                                   |
| ------------- | ------ | ----------- | --------------------------------------- |
| Mesh          |        | _mesh_lod0* | Only use LOD suffix if model uses LOD's |
| Mesh Collider |        | _collider   |                                         |

<a name="anc-animations"></a>
#### 2.2.2 Animations 
| Asset Type           | Prefix | Suffix | Notes |
| -------------------- | ------ | ------ | ----- |
| Animation Clip       | A_     |        |       |
| Animation Controller | AC_    |        |       |
| Avatar Mask          | AM_    |        |       |
| Morph Target         | MT_    |        |       |

<a name="anc-ai"></a>
#### 2.2.3 Artificial Intelligence
| Asset Type              | Prefix       | Suffix     | Notes                            |
| ----------------------- | ------------ | ---------- | -------------------------------- |
| AI / NPC                | AI_          |  _NPC      |   *Npc could be pawn of CH_ !AI_ |                         |
| Behavior Tree           | BT_          |            |                                  |
| Blackboard              | BB_          |            |                                  |
| Decorator               | BTDecorator_ |            |                                  |
| Service                 | BTService_   |            |                                  |
| Task                    | BTTask_      |            |                                  |
| Environment Query       | EQS_         |            |                                  |
| EnvQueryContext         | EQS_         | Context    |                                  |

<a name="anc-prefab"></a>
#### 2.2.4 Prefabs
| Asset Type              | Prefix     | Suffix     | Notes                            |
| ----------------------- | ---------- | ---------- | -------------------------------- |
| Prefab                  |            |            |                                  |
| Prefab Instance         | I          |            |                                  |
| Scriptable Object       |            |            | Assigned "Blueprint" label in Editor |

<a name="anc-materials"></a>

#### 2.2.5 Materials
| Asset Type            | Prefix | Suffix | Notes |
| --------------------- | ------ | ------ | ----- |
| Material              | M_     |        |       |
| Material Instance     | MI_    |        |       |
| Physical Material     | PM_    |        |       |
| Material Shader Graph | MSG_   |        |       |

<a name="anc-textures"></a>

#### 2.2.6 Textures
| Asset Type              | Prefix     | Suffix     | Notes                            |
| ----------------------- | ---------- | ---------- | -------------------------------- |
| Texture                 | T_         |            |                                  |
| Texture (Base Color)    | T_         | _BC        | Diffuse / Albedo     	           |
| Texture (Metallic / Smoothness)| T_  | _MS        |                                  |
| Texture (Normal)        | T_         | _N         |                                  |
| Texture (Alpha)         | T_         | _A         |                                  |
| Texture (Height)        | T_         | _H         |                                  |
| Texture (Ambient Occlusion) | T_     | _AO        |                                  |
| Texture (Emissive)      | T_         | _E         |                                  |
| Texture (Mask)          | T_         | _M         |                                  |
| Texture (Packed)        | T_         | _*         | See notes below about [packing](#anc-textures-packing). |
| Texture Cube            | TC_        |            |                                  |
| Media Texture           | MT_        |            |                                  |
| Render Target           | RT_        |            |                                  |
| Cube Render Target      | RTC_       |            |                                  |
| Texture Light Profile   | TLP_       |            |                                  |

**[⬆ Back to Top](#table-of-contents)**

----------------------------

<a name="asset-workflows"></a>
## 3. Asset Workflows
This section describes best practices for creating and importing assets usable in Unity.

### Sections
> 3.1 [Unity Asset Import Settings](#asset-import-settings)

> 3.2 [Textures](#texture-import)

> 3.3 Audio(#audio-import)

<a name="asset-import-settings"></a>
### 3.1 Unity Asset Import Settings
Unity's [Asset Postprocessor](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html) lets you hook into the import pipeline and run scripts prior to or after importing assets. This allows you to enforce import settings when assets are first imported into the project. For example textures that end with `_N` can be marked as Normal Map on import.

**References:**
[World of Zero - Automatically Processing Assets with Custom Unity Asset Pipelines](https://www.youtube.com/watch?v=8eFaLtJMsNo) (Youtube)
[Example Guide for Import Settings](https://github.com/justinwasilenko/Unity-AssetPostProcessor) (Github)

<a name="texture-import"></a>
### 3.2 Textures
* Textures follow the [naming convention](#anc-textures) found above.
* Tehy are a power of two (For example 512x512 or 256x1024).
* Use Texture Atlases wherever possible.
* 3D software should point directly to the Unity project textures for consistency when you save or export.
* It is better to resize the texture in Photoshop then to use Unity's compresssion options when the in game texture resolution is already known. This reduces the file size and import time of the texture in Unity.
* When working with a high-resolution source PSD outside the Unity project, use the samew name for both the high-resolution and the imported Unity file. This allows quick iteration when swapping between the 2 textures.

More information for importing textures can be found [here](https://docs.unity3d.com/Manual/ImportingTextures.html) (Unity Documentation).

Textures requiring the use of a Alpha channel should follow [this guide](https://docs.unity3d.com/Manual/HOWTO-alphamaps.html) (Unity Documentation).

#### 3.2.1 Texture File Format
All textures should be of the .PSD format. No layers should be included and only one Alpha channel in the imported file.

<a name="audio-import"></a>
### 3.3 Audio
Only import uncompressed audio files in to Unity using WAV or AIFF formats. They will be compressed automatically by the [Asset Post Processor](#asset-import-settings).

For further information read this detailed [Audio Import Optimization Guide](https://www.gamedeveloper.com/audio/unity-audio-import-optimisation---getting-more-bam-for-your-ram) (Article)

**[⬆ Back to Top](#table-of-contents)**

----------------------------

<a name="scripts"></a>
## 4. Scripts
This section will focus on C# classes and their internals. When possible, style rules conform to Microsoft's C# standard.

### Sections
> 4.1 [Class Organization](#classorganization)

> 4.2 [Compiling](#compiling)

> 4.3 [Variables](#variables)

> 4.4 [Functions](#functions)

<a name="classorganization"></a>
### 4.1 Class Organization
Source files should contain only one public type, although multiple internal classes are allowed.

* Source files should be given the name of the public class in the file.
* Organize namespaces with a clearly defined structure.

Class members should be alphabetized, and grouped into sections:
* Constant Fields
* Static Fields
* Fields
* Constructors
* Properties
* Events / Delegates
* LifeCycle Methods (Awake, OnEnable, OnDisable, OnDestroy)
* Public Methods
* Private Methods
* Nested types

Within each of these groups order by access:
* public
* internal
* protected
* private
```
namespace ProjectName
{
	/// <summary>  
	/// Brief summary of what the class does
	/// </summary>
    public class Account
    {
      #region Fields
      [Tooltip("Public variables set in the Inspector, should have a Tooltip")]
      public static string BankName;
      
	  /// <summary>  
	  /// They should also have a summary
	  /// </summary>
      public static decimal Reserves;
 
	  public string BankName;
	  public const string ShippingType = "DropShip";
	  
	  private float _timeToDie;
	  #endregion
	  
	  #region Properties
      public string Number {get; set;}
      public DateTime DateOpened {get; set;}
      public DateTime DateClosed {get; set;}
      public decimal Balance {get; set;}
	  #endregion
	 
	  #region LifeCycle
      public Awake()
      {
        // ...
      }
      #endregion

	  #region Public
      ///<summary>
      /// Should also have a summary
      ///</summary///
      public AddObjectToBank()
      {
        // ...
      }
      #endregion

      #region Private
      void CalculateBalance()
      {
        // ...
      }
      #endregion
    }
}
```

#### Script Templates
To save some time you can overwrite Unity's default script template with your own  to automatically setup the namespace and regions etc. See this Unity [support](https://support.unity3d.com/hc/en-us/articles/210223733-How-to-customize-Unity-script-templates) article to learn how.

<a name="namespace"></a>
#### Namespace
Use a namespace to ensure your scoping of classes/enum/interface/etc won't conflict with existing ones from other namespaces or the global namespace. The project should at minimum use the projects name for the Namespace to prevent conflicts with any imported Third Party assets.

#### All Public Functions Should Have A Summary
Simply, any function that has an access modifier of Public should have its summary filled out. 

```
/// <summary>
/// Fire a gun
/// </summary>
public void Fire()
{
// Fire the gun.
}
```

#### Foldout Groups
If a class has only a small number of variables, Foldout Groups are not required.

If a class has a moderate amount of variables (5-10), all [Serializable](#serializable) variables should have a non-default Foldout Group assigned. A common category is `Config`.

To create Foldout Groups there are 2 options in Unity. 

* The first is to define a `[Serializable] public Class` inside the main class however this can have a performance impact. This allows the use of the same variable name to be shared.
* The second option is to use the Foldout Group Attribute available with [Odin Inspector](https://odininspector.com/).

```
[[Serializable](https://docs.unity3d.com/ScriptReference/Serializable.html)]
public struct PlayerStats
	{
        public int MovementSpeed;
    }
    
[FoldoutGroup("Interactable")]
public int MovementSpeed = 1;
```

#### Commenting
Comments should be mainly used to link to references that inspired a section of your code. See [referencing](#code-references).

As a general rule, try to use as little comments as possible. Comments shouldn't be used to explain your code. If you feel like your code needs an explanation, it's usually a sign that it can be simplified. Ask yourself the following questions to improve readability of your code:
* Can I Improve The [Variable Naming](#code-variables-naming) or [Function Naming](#functions)?
* Is My [Function](#functions) Doing One Thing Only?
* Can I Split Some Lines Up To Make It More Readable?

If you can't improve your code further, but it's still not readable, leave a comment. Comments should be used to describe intention, algorithmic overview, and/or logical flow. It would be ideal if from reading the comments alone someone other than the author could understand a function’s intended behavior and general operation. It shoudl reflect the programmers intention.

##### Comment Style
Place the comment on a separate line, not at the end of a line of code.

* Begin comment text with an uppercase letter.
* End comment text with a period.
* Insert one space between the comment delimiter (//) and the comment text, as shown in the following example.

The // (two slashes) style of comment tags should be used in most situations. Where ever possible, place comments above the code instead of beside it. Here are some examples:
```
// Sample comment above a variable.
private int _myInt = 5;
```


<a name="code-references"></a>
### References
Whenever you found an external reference that inspired a solution to a more complex coding problem, make sure to reference the source with a quick comment mentioning the author, title and the link to the source. If its a longer video, make sure to provide a timestamp to ensure the important section is easy to find.
```
// REFERENCE: 
// Game Dev Guide - How to Build a Save System in Unity: https://www.youtube.com/watch?v=5roZtuqZyuw (06:10)
static BinaryFormatter GetBinaryFormatter() { 
    //... 
}
```

By referencing we insure that the code is easy to comprehend in the future. Also its just good manners to tribute others.

#### Regions
The `#region` directive enables you to collapse and hide sections of code in C# files. The ability to hide code selectively makes your files more manageable and easier to read. 
```
#region "This is the code to be collapsed"
    Private components As System.ComponentModel.Container
#endregion
```

#### Spacing
Do use a single space after a comma between function arguments.

Example: `Console.In.Read(myChar, 0, 1);`
* Do not use a space after the parenthesis and function arguments.
* Do not use spaces between a function name and parenthesis.
* Do not use spaces inside brackets.
<a name="3.1"></a>
<a name="compiling"></a>
### 4.2 Compiling
All scripts should compile with zero warnings and zero errors. You should fix script warnings and errors immediately as they can quickly cascade into very scary unexpected behavior.

Do *not* submit broken scripts to source control. If you must store them on source control, shelve them instead.

### 4.3 Variables
The words `variable` and `property` may be used interchangeably.

<a name="code-variable-naming"></a>
#### Variable Naming

##### Nouns
All non-boolean variable names must be clear, unambiguous, and descriptive nouns. 

##### Case
All variables use PascalCase unless marked as [private](#privatevariables) which use camelCase. 

Use PascalCase for abbreviations of 4 characters or more (3 chars are both uppercase).

##### Considered Context
All variable names must not be redundant with their context as all variable references in the class will always have context.

###### Considered Context Examples:
Consider a Class called `PlayerCharacter`.

**Bad**

* `PlayerScore`
* `PlayerKills`
* `MyTargetPlayer`
* `MyCharacterName`
* `CharacterSkills`
* `ChosenCharacterSkin`

All of these variables are named redundantly. It is implied that the variable is representative of the `PlayerCharacter` it belongs to because it is `PlayerCharacter` that is defining these variables.

**Good**

* `Score`
* `Kills`
* `TargetPlayer`
* `Name`
* `Skills`
* `Skin`

#### Variable Access Level
In C#, variables have a concept of access level. Public means any code outside the class can access the variable. Protected means only the class and any child classes can access this variable internally. Private means only this class and no child classes can access this variable.
Variables should only be made public if necessary.

Prefer to use the attribute `[SerializeField]` instead of making a variable public.

##### Local Variables
Local variables should use camelCase.

###### Implicitly Typed Local Variables
Use implicit typing for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important.
```
var var1 = "This is clearly a string.";
var var2 = 27;
var var3 = Convert.ToInt32(Console.ReadLine());
// Also used in for loops
for (var i = 0; i < bountyHunterFleets.Length; ++i) {};
```

Do not use var when the type is not apparent from the right side of the assignment.
Example
```
int var4 = ExampleClass.ResultSoFar();
```

<a name="privatevariables"></a>
##### Private Variables
Private variables should have a prefix with a underscore `_myVariable` and use camelCase.

Unless it is known that a variable should only be accessed within the class it is defined and never a child class, do not mark variables as private. Until variables are able to be marked `protected`, reserve private for when you absolutely know you want to restrict child class usage.

##### Do _Not_ use Hungarian notation
Do _not_ use Hungarian notation or any other type identification in identifiers
```
// Correct
int counter;
string name;
 
// Avoid
int iCounter;
string strName;
```

#### Don't use Getters and Setters
Instead of using a public Getter and a Private Setter, use C#'s short form:
```
public List<Threat> Threats {get; private set};
```

#### Use SerializedField for the Inpsector
if you want to expose a variable in the inspector, instead of making it public you should use [SerializedField]. This way the variable is visible in the inspector but not accessable by other scripts.
```
[SerializedField] List<Threat> Threats;
```

##### Tooltips 
All [Serializable](#serializable) variables should have a description in their `[Tooltip]` fields that explains how changing this value affects the behavior of the script.

##### Variable Slider And Value Ranges
All [Serializable](#serializable) variables should make use of slider and value ranges if there is ever a value that a variable should _not_ be set to.

Example: A script that generates fence posts might have an editable variable named `PostsCount` and a value of -1 would not make any sense. Use the range fields `[Range(min, max)]` to mark 0 as a minimum.

If an editable variable is used in a Construction Script, it should have a reasonable Slider Range defined so that someone can not accidentally assign it a large value that could crash the editor.

A Value Range only needs to be defined if the bounds of a value are known. While a Slider Range prevents accidental large number inputs, an undefined Value Range allows a user to specify a value outside the Slider Range that may be considered 'dangerous' but still valid.

#### Variable Types

##### Booleans

###### Boolean Prefix
All booleans should be named in PascalCase but prefixed with a verb.

Example: Use `isDead` and `hasItem`, **not** `Dead` and `Item`.

###### Boolean Names
All booleans should be named as descriptive adjectives when possible if representing general information.

Try to not use verbs such as `isRunning`. Verbs tend to lead to complex states.

###### Boolean Complex States
Do not use booleans to represent complex and/or dependent states. This makes state adding and removing complex and no longer easily readable. Use an enumeration instead.

Example: When defining a weapon, do **not** use `isReloading` and `isEquipping` if a weapon can't be both reloading and equipping. Define an enumeration named `WeaponState` and use a variable with this type named `WeaponState` instead. This makes it far easier to add new states to weapons.

##### Enums
Enums use PascalCase and use singular names for enums and their values. Exception: bit field enums should be plural. Enums can be placed outside the class space to provide global access.

Example: 
```
public enum WeaponType
{
    Knife,
    Gun
}

// Enum can have multiple values
[Flags]
public enum Dockings
{
	None = 0,
	Top = 1,
}

public WeaponType Weapon
```

##### Arrays
Arrays follow the same naming rules as above, but should be named as a plural noun.

Example: Use `Targets`, `Hats`, and `EnemyPlayers`, not `TargetList`, `HatArray`, `EnemyPlayerArray`.

##### Interfaces
Interfaces are led with a capital `I` then followed with PascalCase.

Example: ```public interface ICanEat { }```

<a name="functions"></a>
### 3.4 Functions, Events, and Event Dispatchers
This section describes how you should author functions, events, and event dispatchers. Everything that applies to functions also applies to events, unless otherwise noted.

### Every Function Should Do One Thing Only
To ensure your code is organized and easy to read, make sure that every function does exactly one thing and one thing only. If you have a function that does multiple things at once, its considered bad code. You should split it up or come up with a different approach if not possible.

<a name="function-naming"></a>
#### Function Naming
The naming of functions, events, and event dispatchers is critically important. Based on the name alone, certain assumptions can be made about functions. For example:

* Is it a pure function?
* Is it fetching state information?
* Is it a handler?
* What is its purpose?

These questions and more can all be answered when functions are named appropriately.

<a name="function-verbrule"></a>
#### All Functions Should Be Verbs
All functions and events perform some form of action, whether its getting info, calculating data, or causing something to explode. Therefore, all functions should start with verbs. They should be worded in the present tense whenever possible. They should also have some context as to what they are doing.

Good examples:

* `Fire` - Good example if in a Character / Weapon class, as it has context. Bad if in a Barrel / Grass / any ambiguous class.
* `Jump` - Good example if in a Character class, otherwise, needs context.
* `Explode`
* `ReceiveMessage`
* `SortPlayerArray`
* `GetArmOffset`
* `GetCoordinates`
* `UpdateTransforms`
* `EnableBigHeadMode`
* `IsEnemy` - ["Is" is a verb.](http://writingexplained.org/is-is-a-verb)

Bad examples:

* `Dead` - Is Dead? Will deaden?
* `Rock`
* `ProcessData` - Ambiguous, these words mean nothing.
* `PlayerState` - Nouns are ambiguous.
* `Color` - Verb with no context, or ambiguous noun.

#### Functions Returning Bool Should Ask Questions
When writing a function that does not change the state of or modify any object and is purely for getting information, state, or computing a yes/no value, it should ask a question. This should also follow [the verb rule](#function-verbrule).

This is extremely important as if a question is not asked, it may be assumed that the function performs an action and is returning whether that action succeeded.

Good examples:

* `IsDead`
* `IsOnFire`
* `IsAlive`
* `IsSpeaking`
* `IsHavingAnExistentialCrisis`
* `IsVisible`
* `HasWeapon` - ["Has" is a verb.](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html)
* `WasCharging` - ["Was" is past-tense of "be".](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html) Use "was" when referring to 'previous frame' or 'previous state'.
* `CanReload` - ["Can" is a verb.](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html)

Bad examples:

* `Fire` - Is on fire? Will fire? Do fire?
* `OnFire` - Can be confused with event dispatcher for firing.
* `Dead` - Is dead? Will deaden?
* `Visibility` - Is visible? Set visibility? A description of flying conditions?

#### Event Handlers and Dispatchers Should Start With `On`
Any function that handles an event or dispatches an event should start with `On` and continue to follow [the verb rule](#function-verbrule).

Good examples:

* `OnDeath` - Common collocation in games
* `OnPickup`
* `OnReceiveMessage`
* `OnMessageRecieved`
* `OnTargetChanged`
* `OnClick`
* `OnLeave`

Bad examples:

* `OnData`
* `OnTarget`

**[⬆ Back to Top](#table-of-contents)**

----------------------------

<a name="code-workflows"></a>
## 5. Code Workflows

This section is a guide to best coding practices in Unity. They should be considered as inspiration, you can break these rules if you know why.

### Variable Names
Take your time to come up with good names for your variables. Great variable names ensure good code readability and will save you and your team time in the future. [How to find good variable names](#code-variable-naming).

### Encapsulate Code in Functions
Instead of having methods 100 lines long, break up your logic into functions. And break up those functions into more functions. This way anyone can tell what your program is doing on the highest level. The Update Function for example should only contain functions.
> As a rule of thumb: Any function should only do just one thing and one thing only.

If you use [well named functions](#function-naming), you shouldn't need comment to explain what they are doing.

### Only Write Currently Needed Code
A big trap while writing a script is to prewrite code and functions that you might need in the future. It's a good practice to write code with keeping in mind that you'll probably add some features in the future. But prewriting it wastes time and makes your code more complex than it should be. You can always refactor your code in the future if needed.

### Plan Out Code Before Writing It
Write down pseudo code to plan out what your code should be doing. This extra step is worth it because it will save a lot of time debugging and rewriting code.

### Write Scripts like Components

### Use Architectural Patterns



**[⬆ Back to Top](#table-of-contents)**

----------------------------

<a name="glossary"></a>
## Glossary
This section is a collection of important terminology to look up.

<a name="terms-cases"></a>
### Cases
There are a few different ways you can name things. Here are some common casing types:

> <a name="terms-cases-pascal"></a>
> ##### PascalCase
>Capitalize every word and remove all spaces, e.g. `DesertEagle`, `StyleGuide`, `ASeriesOfWords`.
>
> <a name="terms-cases-camel"></a>
> #### camelCase
> The first letter is always lowercase but every following word starts with uppercase, e.g. `desertEagle`, `styleGuide`, `aSeriesOfWords`.
>
>> <a name="terms-cases-lower"></a>
> #### lowercase
> All letters are lowercase, e.g. `deserteagle`, 
>
> <a name="terms-cases-snake"></a>
> #### Snake_case
> Words can arbitrarily start upper or lowercase but words are separated by an underscore, e.g. `desert_Eagle`, `Style_Guide`, `a_Series_of_Words`.

**[⬆ Back to Top](#table-of-contents)**