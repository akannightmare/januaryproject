# 🟨 ULTIMATE 2D PLATFORMER TOOLKIT

The Ultimate 2D Platformer Toolkit is a modular, plug-and-play controller system for Unity, built to deliver **smooth**, **responsive**, and **feature-rich** character movement for any 2D platformer.

Designed for **rapid integration** and **deep customization**, this package helps you implement polished movement mechanics like wall jumps, dashes, ledge climbing, and more - with clean code and flexible settings.

---

## ⚙️ Key Features

- ✅ Smooth movement with acceleration & deceleration curves  
- ✅ Coyote time & jump buffering for forgiving controls  
- ✅ Wall jump & wall slide  
- ✅ Dash (horizontal, vertical, or both) with optional gravity cancel  
- ✅ Ledge grab & climb (auto or manual)  
- ✅ Variable jump height  
- ✅ Slide mechanic  
- ✅ Modular code and inspector-friendly design  
- ✅ Bonus: Scene view spawn helper & fall-reset trigger  

---

## 📦 Getting Started

You can integrate the toolkit in two ways:

### Option 1: Quick Start with the Prefab

1. Drag the **`Player.prefab`** into your scene.  
2. Press **Play** - it just works!  
3. Customize movement settings in the **`Movement2D`** component in the Inspector.

---

### Option 2: Add to Your Own Character

For more control or to integrate into an existing project:

1. Import the package into your Unity project.
2. Create an empty GameObject (e.g., `Player`).
3. Attach the **`Movement2D.cs`** script to the parent GameObject.
4. Add a child GameObject with a **SpriteRenderer** to visually represent your character.
5. (Optional) Add an **Animator** component to the child for animations.
6. Customize all behavior and physics settings via the Inspector.

---

## 🧩 Utility Scripts Included

- **`SetPlayerToSceneViewPosition.cs`**  
  Automatically sets the player's position to the Scene View camera when entering Play Mode - useful for testing from specific locations.

- **`TeleportOnFall.cs`**  
  Teleports the player to a designated position if they fall out of bounds (great for platformer fail-safes).

---

## 🔧 Inspector Customization

The `Movement2D` script includes organized sections for:

- Movement (speed, acceleration, deceleration)
- Jumping (normal, wall jump, variable height)
- Dashing (distance, direction, gravity cancel, cooldown)
- Ledge grabbing and climbing
- Ground, ceiling, and wall detection
- Gravity, coyote time, jump buffer
- State management and gizmo debugging

All parameters are **serialized and editable via Inspector** - no coding required.

---

## 🧪 Compatibility

- Unity **2021.3** or later
- ✅ Works with **URP** and **Built-in RP**
- ✅ C# only - no third-party dependencies

---

## 📣 Support

For questions, bug reports, or suggestions:  
Please contact me via the Unity Asset Store publisher profile or the email listed there.

Happy platforming! 🎮
