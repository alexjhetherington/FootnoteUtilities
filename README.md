# Footnote Utilities

**Chat with me on the Hoverblade Discord**: https://discord.gg/7QJnhyJpJs

**My Website**: http://www.footnotesforthefuture.com/

**How to install**: Copy the FootnoteUtilities folder to your Unity project

Footnote Utilities is a collection of useful and commonly used textures, shaders, scripts, and frameworks for Unity to stop us from writing them over and over again.

There is a wide variety of stuff in here. We wrote most of it; some of it was written by others and modified by us.

The guiding principle here is that everything should be very lightweight. This means small readable classes, and rely on 'The Unity Way' as much as humanly possible.

Components should rely on each other as little as possible, but in some cases this cannot be avoided.

Some assets contained are fully featured. Some are lightweight (but powerful!) alternatives to more fully featured alternatives that exist elsewhere.

We do not have our own solution for some complicated assets that we use very commonly, like tweening libraries. See the section below that offers suggestions for those.

## Contents

This is an incomplete guide to the contents of Footnote Utilities, highlighting important and interesting features.

### Attributes

* Add the [Button] attribute to a method on a Monobehaviour to expose a button in the inspector

### Data Structures

* Use a TimeArray to discard elements after the given amount of time has passed. It must be manually ticked
* Use PoolManager.SpawnObject and PoolManager.ReleaseObject for object pooling

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

### Scriptable Object Architecture

See Ryan Hipple's [Talk] (https://www.youtube.com/watch?v=raQ3iHhE_Kk) on the scriptable object component architecture.

In summary, this implementation offers:

* ScriptableObject based events
* ScriptableObject properties that include onChanged events (especially useful for UI)
* MonoBehavious can extend PartOfMonoBehaviourRuntimeSet to be included in a ScriptableObject set

### Shaders

* HorizontalSkybox is just magnificently useful
* PortalCard allows you to draw fog like quads that fade when the player gets close to them. Very useful for atmosphere

### Sound Manager

Create the folder Resources/SoundManager. Inside put instantiations of the SoundLibrary ScriptableObject. SoundLibrary has a simple but clean interface that allows you to easily manager your sounds all in one place, including volume and pitch adjustment.

Sounds can be played by calling SoundManager.Play. Various overrides allow the sound to have an override pitch, volume, follow another gameobject etc.

Sounds can be assigned an alias name. If multiple sounds have the same alias name, one will be selected random for play.

### Textures

* Some simple noise textures for prototyping
* A rounded rectangle for cartoonish UI
* A fake cubemap - allows materials using the standard shader to have a certain nice effect with the right light renderer settings

### Unity Events

Generic classes cannot be serialised, but extending classes that define the generic type can.

### Utilities

* Various more complex math functions in Footnote3D
* Find all objects of type including disabled in FootnoteHack. Should be avoided! But it's there in case you need it :)

## Other Libraries we like

* Level Design: [RealtimeCsg] (https://github.com/LogicalError/realtime-CSG-for-unity/)
* Tweening: [ZestKit](https://github.com/prime31/ZestKit)
* Screen Transitions: [TransitionKit] (https://github.com/prime31/TransitionKit)

Easy and Unity style dependency injection: [Medicine] (https://github.com/apkd/Medicine)

## Attribution

* Object Pooling: https://github.com/thefuntastic/unity-object-pool
* KinematicBody: Extensively modified version of: https://github.com/marmitoTH/Unity-Kinematic-Body
* Button Attribute: https://github.com/dbrizov/NaughtyAttributes
* Fix Class Name: http://www.pellegrinoprincipe.com/
* Light Flicker: https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
* ShakeableTransform: https://github.com/IronWarrior/UnityCameraShake/blob/master/Assets/ShakeableTransform.cs
* Sokpop Shaders: https://www.youtube.com/watch?v=XatLA5SGgAs
* Waypoint Gizmos: https://github.com/SebLague/Intro-to-Gamedev/blob/master/Episode%2023/Assets/Guard.cs