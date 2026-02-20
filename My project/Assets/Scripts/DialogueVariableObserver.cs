using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using System.IO;

/// <summary>
///  This script will keep track of player choices and save related data (to be referenced later/in other ink "stories")
///  resources: https://www.youtube.com/watch?v=fA79neqH21s&list=PL3viUl9h9k78KsDxXoAzgQ1yRjhm7p8kl&index=5
/// </summary>
public class DialogueVariableObserver 
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    public DialogueVariableObserver(string globalIsFilePath)
    {
        // compile into story 
        string inkFileContents = File.ReadAllText(globalIsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        // initialize dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState) // populate dictionary with the variables stored in the ink story we just created
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void StartListening(Story story) // to be called when dialogue starts
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }
    public void StopListening(Story story) // to be called when dialogue ends
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        Debug.Log("Variable changed: " + name + " = " +  value);


        // only maintains variables stored in the global ink file
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value); // update variable value if it exists
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
