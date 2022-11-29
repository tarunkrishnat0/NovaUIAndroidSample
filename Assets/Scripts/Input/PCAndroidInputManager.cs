using Nova;
using UnityEngine;

namespace NovaSamples.Inventory
{
    public class PCAndroidInputManager : InputManager
    {
        /// <summary>
        /// The controlID for mouse point events
        /// </summary>
        public const uint MousePointerControlID = 1;

        /// <summary>
        /// The controlID for mouse wheel events
        /// </summary>
        public const uint ScrollWheelControlID = 2;

        /// <summary>
        /// To store the button states of both the left and right mouse buttons.
        /// </summary>
        private static readonly InputData Data = new InputData();

        [Tooltip("Inverts the mouse wheel scroll direction.")]
        public bool InvertScrolling = true;

        /// <summary>
        /// The camera used to convert a mouse position into a world ray
        /// </summary>
        private Camera cam = null;

        /// <summary>
        /// The camera used to convert a mouse position into a world ray
        /// </summary>
        private Camera Cam
        {
            get
            {
                if (cam == null)
                {
                    // Cache it so we don't need to requery every frame.
                    cam = Camera.main;
                }

                return cam;
            }
        }

        //private void Awake()
        //{
        //    QualitySettings.vSyncCount = 0;
        //    Application.targetFrameRate = 60;
        //}

        public override bool TryGetRay(uint controlID, out Ray ray)
        {
            if (controlID != MousePointerControlID)
            {
                ray = default;
                return false;
            }

            ray = Cam.ScreenPointToRay(Input.mousePosition);
            return true;
        }

        private void Update()
        {
#if UNITY_EDITOR
            UpdatePC();
#else
            UpdateAndroid();
#endif
        }

        private void UpdateAndroid()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Using Unity's legacy input system, get the first touch point.
                Touch touch = Input.GetTouch(i);

                // Convert the touch point to a world-space ray.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Create a new Interaction from the ray and the finger's ID
                Interaction.Update interaction = new Interaction.Update(ray, (uint)touch.fingerId);

                // Get the current touch phase
                TouchPhase touchPhase = touch.phase;

                // If the touch phase hasn't ended and hasn't been canceled, then pointerDown == true.
                bool pointerDown = touchPhase != TouchPhase.Canceled && touchPhase != TouchPhase.Ended;

                // Feed the update and pressed state to Nova's Interaction APIs
                Interaction.Point(interaction, pointerDown);
            }
        }

        private void UpdatePC()
        {
            if (!Input.mousePresent)
            {
                // Nothing to do, no mouse device detected
                return;
            }

            // Get the current world-space ray of the mouse
            Ray mouseRay = Cam.ScreenPointToRay(Input.mousePosition);

            // Get the current scroll wheel delta
            Vector2 mouseScrollDelta = Input.mouseScrollDelta;

            // Check if there is any scrolling this frame
            if (mouseScrollDelta != Vector2.zero)
            {
                // Invert scrolling for a mouse-type experience,
                // otherwise will scroll track-pad style.
                if (InvertScrolling)
                {
                    mouseScrollDelta.y *= -1f;
                }

                // Create a new Interaction.Update from the mouse ray and scroll wheel control id
                Interaction.Update scrollInteraction = new Interaction.Update(mouseRay, ScrollWheelControlID);

                // Feed the scroll update and scroll delta into Nova's Interaction APIs
                Interaction.Scroll(scrollInteraction, mouseScrollDelta);
            }

            // Store the button states for left/right mouse buttons
            Data.PrimaryButtonDown = Input.GetMouseButton(0);
            Data.SecondaryButtonDown = Input.GetMouseButton(1);
            //Debug.Log($"PrimaryButtonDown = {Data.PrimaryButtonDown}, SecondaryButtonDown={Data.SecondaryButtonDown}");

            // Create a new Interaction.Update from the mouse ray and pointer control id
            Interaction.Update pointInteraction = new Interaction.Update(mouseRay, MousePointerControlID, userData: Data);

            // Feed the pointer update and pressed state to Nova's Interaction APIs
            Interaction.Point(pointInteraction, Data.AnyButtonPressed);
        }
    }
}

