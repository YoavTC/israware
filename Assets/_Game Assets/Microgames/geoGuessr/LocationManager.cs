using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using External_Packages.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game_Assets.Microgames.geoGuessr
{
    public class LocationManager : MonoBehaviour
    {
        [Header("Locations")]
        [SerializeField] private SerializedDictionary<string, Sprite[]> locationSpritesDictionary;
        [SerializeField] private string[] extraLocations;

        [Header("Components")]
        [SerializeField] private Image locationImageDisplay;
        [SerializeField] private Button[] locationButtons;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<bool> locationButtonPressedUnityEvent;
        
        private string[] locations;
        private string correctLocation;
        
        private void Start()
        {
            SetupLocations();
        }

        private void SetupLocations()
        {
            List<string> allLocations = locationSpritesDictionary.Keys.ToList();
            
            // Select a random location from the dictionary
            correctLocation = allLocations.Random();
            locationImageDisplay.sprite = locationSpritesDictionary[correctLocation].Random();
            
            allLocations.AddRange(extraLocations); // Add extra locations to the list
            allLocations.Remove(correctLocation); // Remove the correct location from the list
            allLocations.Shuffle(); 
            
            // Select 3 random locations from the list & add the correct location
            string[] finalLocations = allLocations.Take(3).Append(correctLocation).ToArray();
            finalLocations.Shuffle();

            for (int i = 0; i < finalLocations.Length; i++)
            {
                // Store changing index variable in as local to avoid closure issues
                int index = i;
                
                // Set the button text and add listener
                locationButtons[index].GetComponentInChildren<TMP_Text>().text = finalLocations[index];
                locationButtons[index].onClick.AddListener(() => OnLocationButtonPressed(locationButtons[index], finalLocations[index]));
            }
        }

        private void OnLocationButtonPressed(Button button, string location)
        {
            // Check if the pressed button is the correct location
            bool correct = correctLocation == location;
            button.image.color = correct ? Color.green : Color.red;
            
            // Fire the event with the result
            locationButtonPressedUnityEvent?.Invoke(correct);
            
            // Disable all buttons interactions
            locationButtons.ForEach(locationButton => locationButton.interactable = false);
        }
    }
}
