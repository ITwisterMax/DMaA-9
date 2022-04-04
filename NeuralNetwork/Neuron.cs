using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    /// <summary>
    ///     Neuron class
    /// </summary>
    [Serializable]
    internal class Neuron
    {
        /// <summary>
        ///     Vactor size
        /// </summary>
        private readonly int _vectorSize;

        /// <summary>
        ///     Weight
        /// </summary>
        private readonly List<int> _weight;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="vectorSize">Vector size</param>
        public Neuron(int vectorSize)
        {
            _vectorSize = vectorSize;
            _weight = GetRandomWeight(vectorSize);
        }

        /// <summary>
        ///     Get random weight
        /// </summary>
        ///
        /// <param name="vectorSize">Vector size</param>
        ///
        /// <returns>List<int></returns>
        private static List<int> GetRandomWeight(int vectorSize)
        {
            var random = new Random();
            var weight = new List<int>();

            for (var i = 0; i < vectorSize; i++)
            {
                weight.Add(random.Next(10));
            }

            return weight;
        }

        /// <summary>
        ///     Get answer
        /// </summary>
        ///
        /// <param name="element">Element</param>
        ///
        /// <returns>int</returns>
        public int GetAnswer(IEnumerable<int> element)
        {
            return element.Select((x, i) => x * _weight[i]).Sum();
        }

        /// <summary>
        ///     Teaching
        /// </summary>
        ///
        /// <param name="element">Element</param>
        /// <param name="factor">Factor</param>
        public void Teaching(List<int> element, int factor)
        {
            for (var i = 0; i < _vectorSize; i++)
            {
                _weight[i] += factor * element[i];
            }
        }
    }
}