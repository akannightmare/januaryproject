using UnityEditor;
using UnityEngine;

namespace Peykarimeh.PlatformerToolkit
{
    [CustomEditor(typeof(Movement2D))]
    public class Movement2DEditor : Editor
    {
        float headerTopSpace = 50f;
        float headerBottomSpace = 10f;

        bool _jumpDebug;
        bool _wallSlideDebug = false;
        bool _speedDebug = false;
        bool _ledgeDebug;
        bool _onCeilDebug;
        bool _onGroundDebug;
        bool _onWallDebug;

        public override void OnInspectorGUI()
        {
            Movement2D movement2D = (Movement2D)target;

            serializedObject.Update();

            GUIStyle mainHeader = movement2D.mainHeaderStyle;
            mainHeader.normal.textColor = new Color(0.85f, 0.25f, 0.19f);

            headerTopSpace = EditorGUILayout.FloatField(new GUIContent("HeaderTopSpace", "Top space for header"), headerTopSpace);
            headerBottomSpace = EditorGUILayout.FloatField(new GUIContent("HeaderBottomSpace", "Bottom space for header"), headerBottomSpace);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("mainHeaderStyle"), new GUIContent("Main Header Style", "Style for main headers"));


            EditorGUILayout.PropertyField(serializedObject.FindProperty("spriteTransform"), new GUIContent("Sprite Transform", "Transform of the sprite"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rb2"), new GUIContent("Rigidbody2D", "Rigidbody2D component"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleCollider"), new GUIContent("Capsule Collider", "Capsule Collider component"));

            // SPEED HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Speed Values", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"), new GUIContent("Movement Speed", "Max speed of the player"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedUpDuration"), new GUIContent("Speed Up Duration", "Duration for speed up"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedDownDuration"), new GUIContent("Speed Down Duration", "Duration for speed down when moving in opposite direction of input"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stopDuration"), new GUIContent("Stop Duration", "Duration for stopping when there is no input"));
            _speedDebug = EditorGUILayout.Foldout(_speedDebug, "Debug");
            EditorGUILayout.EndVertical();
            if (_speedDebug)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentHorizontalSpeed"), new GUIContent("Current Horizontal Speed", "Current horizontal speed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentVerticalSpeed"), new GUIContent("Current Vertical Speed", "Current vertical speed"));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();


            // DASH HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Dash", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.BeginVertical("box");
            SerializedProperty _dash = serializedObject.FindProperty("Dash");
            EditorGUILayout.PropertyField(_dash);
            if (_dash.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashButton"), new GUIContent("Dash Button", "Button for dashing"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelDashOnWallHit"), new GUIContent("Cancel Dash On Wall Hit", "Whether dash should be canceled upon hitting a wall."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDistance"), new GUIContent("Dash Distance", "Distance covered by dash."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashDuration"), new GUIContent("Dash Duration", "Duration of the dash"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashStopEffect"), new GUIContent("Dash Stop Effect", "The speed effect applied when the dash is finished. A value of 0 will completely stop the player, while a value of 1 will maintain the dash velocity.\""));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashCooldown"), new GUIContent("Dash Cooldown", "Cooldown for dashing"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashCoolTimer"), new GUIContent("Dash Cool Timer", "Timer for dash cooldown"));

                EditorGUILayout.Space();


                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetDashOnGround"), new GUIContent("Reset Dash On Ground", "Determines whether the ability to dash is reset when the player touches the ground. If enabled, the player can dash again upon landing on the ground."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("resetDashOnWall"), new GUIContent("Reset Dash On Wall", "Determines if the dash ability resets upon colliding with a wall. When activated, the player can execute another dash after hitting a wall."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("airDash"), new GUIContent("Air Dash", "Allow dash while in the air"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashCancelsGravity"), new GUIContent("Dash Cancels Gravity", "Controls if dashing stops gravity. When enabled, gravity won't pull you down while dashing."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalDash"), new GUIContent("Vertical Dash", "Enables upward or downward dashes."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalDash"), new GUIContent("Horizontal Dash", "Enables dashes to the left or right."));


                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("Adjust the size and position of the sliding collider to suit your character's sliding behavior. Modify these values in the Unity editor by adjusting the collider's scale and offset. Press the button to save the adjusted values here.", MessageType.Info);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderScale"), new GUIContent("Dash Collider Scale", "Determines the size of the dash collider. Changing this value affects how much space the player occupies during a dash, allowing them to fit through tight spaces or slide under objects."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dashColliderOffset"), new GUIContent("Dash Collider Offset", "Controls the position of the dash collider relative to the player's pivot point. Adjusting this value changes where the collider is placed, enabling precise control over sliding maneuvers during a dash."));

                if (GUILayout.Button("Copy Current Collider Values"))
                {
                    movement2D.GetColliderSize();
                }

            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // WALL JUMP HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Wall Jump/Slide", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.BeginVertical("box");
            SerializedProperty _wallJump = serializedObject.FindProperty("WallJump");
            EditorGUILayout.PropertyField(_wallJump);
            if (_wallJump.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wallJumpVelocity"), new GUIContent("Wall Jump Velocity", "Defines the horizontal (x) and vertical (y) velocity vectors for the player when executing a wall jump. "));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wallJumpDecelerationFactor"), new GUIContent("Wall Jump Deceleration Factor", "Adjusts the rate at which the player's velocity decreases after executing a wall jump. Increasing this value slows down the player more quickly, while decreasing it allows for smoother movement control after a wall jump."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("wallSlideSpeed"), new GUIContent("Wall Slide Speed", "Controls how fast the player moves down while sliding on a wall. Lower values mean slower sliding.\""));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("variableJumpHeightOnWallJump"), new GUIContent("Variable Jump Height On Wall Jump", "When enabled, allows the player to adjust jump height during wall jumps, similar to regular jumps."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("inputDelay"), new GUIContent("Input Delay Duration", "Duration of delay before the player can leave a wall or ledge. This prevents accidental falls by requiring the movement button to be held for a short time."));

                _wallSlideDebug = EditorGUILayout.Foldout(_wallSlideDebug, "Debug");
                if (_wallSlideDebug)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isSlidingOnWall"), new GUIContent("Is Sliding On Wall", "Is currently sliding on wall"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("inputDelayTimer"), new GUIContent("Input Delay Timer", "Tracks the input delay duration."));

                }

            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            // JUMP HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Jumping", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpHight"), new GUIContent("Jump Height", "Controls how high the player can jump."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpUpAcceleration"), new GUIContent("Upward Jump Deceleration", "Controls how quickly the player's upward movement decelerates during jumps."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpDownAcceleration"), new GUIContent("Downward Jump Acceleration", "Increases the player's falling speed while descending during jumps."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fallSpeedClamp"), new GUIContent("Maximum Fall Speed", "Limits the player's falling speed to provide more control and prevent excessively fast descents."));

            _jumpDebug = EditorGUILayout.Foldout(_jumpDebug, "Debug");
            if (_jumpDebug)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpVelocity"), new GUIContent("Jump Velocity", "Determines the speed at which the player jumps upward."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gravity"), new GUIContent("Gravity", "Default gravity force applied"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isWallJumped"), new GUIContent("Is Wall Jumped", "Is currently wall jumped"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isNormalJumped"), new GUIContent("Is Normal Jumped", "Is currently normally jumped"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpUpDuration"), new GUIContent("Jump Up Duration", "Duration of jumping up"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpToleranceTimer"), new GUIContent("Jump Tolerance Timer", "Timer for jump tolerance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fallToleranceTimer"), new GUIContent("Fall Tolerance Timer", "Timer for fall tolerance"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("canJump"), new GUIContent("Can Jump", "Can currently jump"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isJumped"), new GUIContent("Is Jumped", "Is currently jumping"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isPressedJumpButton"), new GUIContent("Is Pressed Jump Button", "Is currently pressing jump button"));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // JUMP ADJUSTMENTS HEADER
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Jump Adjustments", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpButton"), new GUIContent("Jump Button", "Button for jumping"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("coyoteTime"), new GUIContent("Coyote Time", "Allows the player to still jump shortly after leaving a platform,making jumps more forgiving."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpBuffer"), new GUIContent("Jump Buffer", "Allows the player to jump even if the jump button is pressed slightly before landing, making jumps more forgiving."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onAirControl"), new GUIContent("On Air Control", "Determines how much control the player has while in the air. A value of 0 means no control, and 1 means full control."));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("variableJumpHeightDuration"), new GUIContent("Variable Jump Height", "Determines how long the player can hold the jump button to increase height while going upward. A value of 1 allows full control for the entire upward movement, while lower values reduce the time for adjusting jump height."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpReleaseEffect"), new GUIContent("Jump Release Effect", "Multiplies the jump velocity to reduce speed when the player releases the jump button. A value between 1 and 0; 0 completely cancels the jump."));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Ledge Grab", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);
            EditorGUILayout.BeginVertical("box");

            SerializedProperty _canGrabLedge = serializedObject.FindProperty("LedgeGrab");
            EditorGUILayout.PropertyField(_canGrabLedge);
            if (_canGrabLedge.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty _autoClimb = serializedObject.FindProperty("autoClimbLedge");
                EditorGUILayout.PropertyField(_autoClimb, new GUIContent("Ledge Auto-Climb", "Automatically climbs the player up when they grab a ledge."));
                if (!_autoClimb.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("climbButton"), new GUIContent("Climb Buton", ""));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("canWallJumpWhileClimbing"), new GUIContent("Can WallJump While Climbing", ""));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbDuration"), new GUIContent("ledge Climb Duration", ""));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheckOffset"), new GUIContent("Ledge Check Offset", "Distance for checking ledge"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheckDistance"), new GUIContent("Ledge Check Distance", "Distance for checking ledge"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheckLayer"), new GUIContent("Ledge Check Layer", "Layer for checking ledge"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbPosOffset"), new GUIContent("Ledge Climb Position Offset", "Offset for ledge climb position"));
                EditorGUILayout.Space();

                _ledgeDebug = EditorGUILayout.Foldout(_ledgeDebug, "Debug");
                if (_ledgeDebug)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isLedge"), new GUIContent("Is Ledge", "Is currently on a ledge"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isClimbingLedge"), new GUIContent("Is Climbing Ledge", "Is currently climbing a ledge"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbTimer"), new GUIContent("ledge Climb Timer", ""));

                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();


            // New fields for ground check
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Ground Check", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheckCenter"), new GUIContent("ground Check Center Position", "Distance for ground check ray"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheckRayDistance"), new GUIContent("Ground Check Ray Distance", "Distance for ground check ray"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundLayer"), new GUIContent("Ground Layer", "Layer for ground check"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheckCircleRadius"), new GUIContent("Ground Check Circle Radius", "Radius for ground check circle"));

            if (GUILayout.Button("Auto-Align"))
            {
                movement2D.GetAutoValueForGroundCheck();
            }
            _onGroundDebug = EditorGUILayout.Foldout(_onGroundDebug, "Debug");
            if (_onGroundDebug)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isGrounded"), new GUIContent("Is Grounded", "Is currently grounded"));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // New fields for ceiling check
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Ceil Check", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilCheckCenter"), new GUIContent("Ceil Check Center Position", "Distance for ground check ray"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilCheckRayDistance"), new GUIContent("Ceil Check Ray Distance", "Distance for ceiling check ray"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilLayer"), new GUIContent("Ceil Layer", "Layer for ceiling check"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ceilCheckCircleRadius"), new GUIContent("Ceil Check Circle Radius", "Radius for ceiling check circle"));

            if (GUILayout.Button("Auto-Align"))
            {
                movement2D.GetAutoValueForCeilCheck();
            }

            _onCeilDebug = EditorGUILayout.Foldout(_onCeilDebug, "Debug");
            if (_onCeilDebug)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("onCeil"), new GUIContent("On Ceiling", "Is currently on ceiling"));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // New fields for wall check
            EditorGUILayout.Space(headerTopSpace);
            EditorGUILayout.BeginVertical("window");
            EditorGUILayout.LabelField("Wall Check", mainHeader);
            EditorGUILayout.Space(headerBottomSpace);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheckRayDistance"), new GUIContent("Wall Check Ray Distance", "Distance for wall check ray"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheckLayer"), new GUIContent("Wall Check Layer", "Layer for wall check"));

            _onWallDebug = EditorGUILayout.Foldout(_onWallDebug, "Debug");
            if (_onWallDebug)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("leftWallHit"), new GUIContent("Left Wall Hit", "Is currently hitting left wall"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rightWallHit"), new GUIContent("Right Wall Hit", "Is currently hitting right wall"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("hitWall"), new GUIContent("Hit Wall", "Is currently hitting any wall"));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // New fields for jump debugging




            EditorGUILayout.Space();

            // New fields for general debugging




            serializedObject.ApplyModifiedProperties();
        }
    }
}