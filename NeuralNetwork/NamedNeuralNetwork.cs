using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    /// <summary>
    ///     Named neural network
    /// </summary>
    [Serializable]
    public class NamedNeuralNetwork
    {
        /// <summary>
        ///     Neural network
        /// </summary>
        private readonly NeuralNetwork _neuralNetwork;

        /// <summary>
        ///     Neurons names
        /// </summary>
        private readonly List<string> _neuronsNames;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="vectorSize">Vector size</param>
        /// <param name="neuronsNames">Neurons names</param>
        public NamedNeuralNetwork(int vectorSize, List<String> neuronsNames)
        {
            _neuronsNames = neuronsNames;
            _neuralNetwork = new NeuralNetwork(vectorSize, neuronsNames.Count);
        }

        /// <summary>
        ///     Teaching
        /// </summary>
        ///
        /// <param name="element">Element</param>
        /// <param name="correctClassName">Correct class name</param>
        public void Teaching(List<int> element, String correctClassName)
        {
            var correctClass = _neuronsNames.FindIndex(x => x == correctClassName);
            if (correctClass == -1)
            {
                throw new ArgumentException("Неверное имя класса!");
            }
            
            _neuralNetwork.Teaching(element, correctClass);
        }

        /// <summary>
        ///     Get answer
        /// </summary>
        ///
        /// <param name="element">Element</param>
        ///
        /// <returns>String</returns>
        public String GetAnswer(List<int> element)
        {
            return _neuronsNames[_neuralNetwork.GetAnswer(element)];
        }
    }
}