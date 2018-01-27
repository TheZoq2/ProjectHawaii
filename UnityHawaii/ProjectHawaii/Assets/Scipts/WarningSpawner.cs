using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WarningSpawner : MonoBehaviour
    {
        public GameObject Prefab;

        private Color[] _calamities =
        {
            Color.blue,
            Color.red,
            Color.green,
            Color.yellow
        };

        private GameObject[] _colorGameObjects;

        void Start()
        {
            EventManager.OnClicked += CalamitiesChanged;
            _colorGameObjects = new []
            {
                GameObject.Find("IslandSelector").transform.Find("Red").gameObject,
                GameObject.Find("IslandSelector").transform.Find("Green").gameObject,
                GameObject.Find("IslandSelector").transform.Find("Blue").gameObject,
                GameObject.Find("IslandSelector").transform.Find("Yellow").gameObject
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SpawnAWarning();
            }
        }

        public void SpawnAWarning()
        {
            GameObject g = Instantiate(Prefab, GameObject.FindGameObjectWithTag("WarningPanel").transform);
            g.transform.localPosition = new Vector3(400, 0);
            var script = g.GetComponent<WarningMovementScript>();

            script.SequenceOrder = Statics.Warnings.Count + 1;
            script.SetCalamity(_calamities[Mathf.FloorToInt(Random.Range(0, 4))]);
            Statics.Warnings.Add(g);
            EventManager.CalamitiesChanged();
        }

        private void CalamitiesChanged()
        {
            HashSet<string> strings = new HashSet<string>();
            GameObject go = GameObject.Find("IslandSelector");

            Statics.Warnings.ForEach(w =>
            {
                strings.Add(HelperFunctions.ParseColorToString(w.GetComponent<Image>().color));
            });

            foreach (GameObject co in _colorGameObjects)
            {
                co.SetActive(strings.Contains(co.name));
            }
        }
    }
}
