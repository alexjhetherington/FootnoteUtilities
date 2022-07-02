# Footnote Utilities

**Chat with me on the Hoverblade Discord**: https://discord.gg/7QJnhyJpJs

**My Website**: http://www.footnotesforthefuture.com/

**How to install**: Copy the root folder to Assets/Plugins

This repo contains 2 related projects. The main project is called Footnote Utilities. The secondary project is called Footnote Framework.

In general, the repo stores common unity assets so we can focus on the novel aspects of projects we build. See the bottom of the readme for other recommended libraries.

#### Footnote Utilities

Footnote Utilities contains a variety of non-specific assets including textures, shaders and scripts. See the later section for a more in depth summary of the contents.

Footnote Utilities are mostly written by us. Some utilities have been written by others; some of those have been modified explicitly by us and some of them remain mostly untouched. 

Each Footnote Utility should be simple and work (mostly) on its own - so one can pick and choose exactly which parts to use in their project. We focus on code-based solutions and try to do things the 'Unity Way' as much as possible.

#### Footnote Framework

Footwork Framework is a base project that uses some Footnote Utilities. It is inside a hidden folder. Go to Tools -> Footnote Framework to generate it. This will copy the hidden folder to the Assets folder. It will require importing TextMeshPro essentials also.

Footwork Framework is more opinionated and may not be suitable for all projects. Even then - it is expected that the imported assets are modified for your specific project. There is no way to create a generic main menu, and it is simpler to just modify one script rather than get lost in abstraction and extension (this is why we copy the base project - the files are now 'yours').

Footwork Framework is still under construction and I would call it very experimental.

## Contents of Footnote Utilities 

This is an incomplete guide to the contents of Footnote Utilities, highlighting important and interesting features.

### Attributes

* Add the [Button] attribute to a method on a Monobehaviour to expose a button in the inspector. Due to the general nature of this attribute I have not implemented undo. Be careful using this at editor time!
* Add the [Resolve] attribute to a public or serialised field to expose automatic resolution buttons in the inspector. N.B. Arrays will always be overriden, but single references will only be resolved if they are null. Existing single references will not be re-resolved)

### Data Structures

* LayerGroup (see note below) to manage layers
* Use a TimeArray to discard elements after the given amount of time has passed. It must be manually ticked
* Use PoolManager.SpawnObject and PoolManager.ReleaseObject for object pooling

Layers are per project static data. To avoid complication but still stay safe I recommend hardcoding layers, referencing static string variables so typos can be fixed all in one place.

This isn't possible for code that you intend to share between projects where layers may be named differently. Use the LayerGroup scriptable object, a reference to which can be exposed to the Unity editor, to write components with layers that can be customised per project while still keeping layers in one place.

### Editor

* If you ever change the file name of a class, you can select the Fix Class Name option in the context menu to automatically match it inside the file

### Extensions

* (MonoBehaviour/GameObject).GetOrAddComponent for easier/safer component management
* LineRenderer.GetPositions for an array of positions in the line
* MonoBehaviour.DoAfter to perform an action after a specified amount of time
* NavMeshAgent.HasArrived
* Vector3.FuzzyEquals
* Vector3.Horizontal and Vector3.Vertical

### MonoBehaviour

* Billboard rotate will make a gameobject cardboard cutout style face the camera
* EntitiesWithinCollider maintains a list of entities within a collider - and correctly handles entities that are destroyed while inside
* LightFlickerEffect
* ScreenSpaceHudHelper contains the (frankly magical) maths needed to position hud markers on the screen based on world position. Works well even at screen edge!
* ShakeableTransform
* SmoothLookAt can be customised to work like a smooth billboard rotate or optionally include looking up and down
* TextureScroller
* KinematicBody - a character controller that uses an arbitrary mesh!
* PlayableAnimationController - trigger non legacy animations from code by directly passing clips
* Behaviour trees - see the README in the Behaviour Tree folder

### Static Scenes

Scenes can be "unpacked" to emulate a sort of "static scene". When unpacked a scene is loaded and all its contents set to DoNotDestroyOnLoad, then the scene is unloaded. If unpacked again, a reference to the existing collection of gameobjects is returned.

This workflow is similar to using singletons but allows for references to be set between game objects easily.

A similar workflow could definitely be achieved using prefabs and game object ids. If it is request, I will add this in future.

### Scriptable Object Architecture

See Ryan Hipple's [Talk](https://www.youtube.com/watch?v=raQ3iHhE_Kk) on the scriptable object component architecture.

In summary, this implementation offers:

* ScriptableObject based events
* ScriptableObject properties that include onChanged events (especially useful for UI)
* MonoBehavious can extend PartOfMonoBehaviourRuntimeSet to be included in a ScriptableObject set

### Shaders

* HorizontalSkybox is just magnificently useful
* PortalCard allows you to draw fog like quads that fade when the player gets close to them. Very useful for atmosphere

### Sound Manager

Create the folder Resources/SoundLibraries. Inside put instantiations of the SoundLibrary ScriptableObject. SoundLibrary has a simple but clean interface that allows you to easily manager your sounds all in one place, including volume and pitch adjustment.

Sounds can be played by calling SoundManager.Play. Various overrides allow the sound to have an override pitch, volume, follow another gameobject etc.

Sounds can be assigned an alias name. If multiple sounds have the same alias name, one will be selected random for play.

### Tags

A super simple tag library with 2 purposes:

* Avoid using strings
* Allow tags to be shared across projects

To define a tag, create a new Tag scriptable object.

To add a tag to a gameobject, add the Tags component, and drag a tag on to it.

To check for tags, call the extension MonoBehaviour.HasTag()

To search for tags easily in the project interface, use t:Tag

### Textures

* Some simple noise textures for prototyping
* A rounded rectangle for cartoonish UI
* A fake cubemap - allows materials using the standard shader to have a certain nice effect with the right light renderer settings

### Transition

Offers a simple API to start transitions. Transitions can perform arbitrary actions when the screen is obscured. They can also switch scene. Call Transitions.Start. The included fade transition demonstrates the Scene Pack workflow.

### UI

Make an instance of the UiSettings scriptable object. It allows you to create menus using code. Use AnchorUtil to set relative positions.

### Unity Events

Generic classes cannot be serialised, but extending classes that define the generic type can.

### Utilities

* Various more complex math functions in Footnote3D
* Find all objects of type including disabled in FootnoteHack. Should be avoided! But it's there in case you need it :)

## Other Libraries we like

* Level Design: [RealtimeCsg](https://github.com/LogicalError/realtime-CSG-for-unity/)
* Tweening: [ZestKit](https://github.com/prime31/ZestKit)
* Easy and Unity style dependency injection: [Medicine](https://github.com/apkd/Medicine)

## Attribution

* Object Pooling: https://github.com/thefuntastic/unity-object-pool
* KinematicBody: Extensively modified version of: https://github.com/marmitoTH/Unity-Kinematic-Body
* Button Attribute: https://github.com/dbrizov/NaughtyAttributes
* Fix Class Name: http://www.pellegrinoprincipe.com/
* Light Flicker: https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
* ShakeableTransform: https://github.com/IronWarrior/UnityCameraShake/blob/master/Assets/ShakeableTransform.cs
* Sokpop Shaders: https://www.youtube.com/watch?v=XatLA5SGgAs
* Waypoint Gizmos: https://github.com/SebLague/Intro-to-Gamedev/blob/master/Episode%2023/Assets/Guard.cs
* UI Blur Shader: https://gist.github.com/JohannesMP/7d62f282705169a2855a0aac315ff381#file-uiblur_shared-cginc
* Scene Reference: https://github.com/NibbleByte/UnitySceneReference