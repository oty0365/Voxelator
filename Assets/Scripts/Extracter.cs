using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Extracter : HalfSingleMono<Extracter> 
{
    public string[] CollectAugmentAsString(GameObject target)
    {
        var augmentComponents = target.GetComponents<MonoBehaviour>();


        var allAugmentStrings = new List<string>();

        foreach (var component in augmentComponents)
        {
            var allFields = component.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var serializableFields = allFields
                .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null);



            var attributedFields = serializableFields
                .Where(f => f.GetCustomAttribute<TextableAugmentAttribute>() != null);

            var fieldStrings = attributedFields
                .Select(f => {
                    var value = f.GetValue(component)?.ToString() ?? "null";
                    return value;
                });

            allAugmentStrings.AddRange(fieldStrings);
        }

        return allAugmentStrings.ToArray();
    }


  public string[] CollectAugmentsAsString(GameObject[] targets)
    {
        var allAugmentStrings = new List<string>();
        
        foreach (var target in targets)
        {
            var augmentComponents = target.GetComponents<MonoBehaviour>();
                
            foreach (var component in augmentComponents)
            {
                var allFields = component.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var serializableFields = allFields
                    .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null);
                var attributedFields = serializableFields
                    .Where(f => f.GetCustomAttribute<TextableAugmentAttribute>() != null);
                var fieldStrings = attributedFields
                    .Select(f => {
                        var value = f.GetValue(component)?.ToString() ?? "null";
                        return value;
                    });
                allAugmentStrings.AddRange(fieldStrings);
            }
        }
        
        return allAugmentStrings.ToArray();
    }


}
