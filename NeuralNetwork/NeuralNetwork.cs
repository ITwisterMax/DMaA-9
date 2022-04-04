using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    /// <summary>
    ///     Neural network
    /// </summary>
    [Serializable]
    public class NeuralNetwork
    {
        /// <summary>
        ///     Threshold
        /// </summary>
        private const int _threshold = 70;

        /// <summary>
        ///     Neurons
        /// </summary>
        private readonly List<Neuron> _neurons;

        /// <summary>
        ///     Neurons count
        /// </summary>
        private readonly int _neuronsCount;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="vectorSize">Vector size</param>
        /// <param name="neuronsCount">Neurons count</param>
        public NeuralNetwork(int vectorSize, int neuronsCount)
        {
            _neuronsCount = neuronsCount;
            _neurons = GetStartNeurons(vectorSize, neuronsCount);
        }

        /// <summary>
        ///     Get start neurons
        /// </summary>
        ///
        /// <param name="vectorSize">Vector size</param>
        /// <param name="neuronsCount">Neurons count</param>
        ///
        /// <returns>List<Neuron></returns>
        private static List<Neuron> GetStartNeurons(int vectorSize, int neuronsCount)
        {
            var neurons = new List<Neuron>();
            for (var i = 0; i < neuronsCount; i++)
            {
                neurons.Add(new Neuron(vectorSize));
            }

            return neurons;
        }

        /// <summary>
        ///     Teaching
        /// </summary>
        ///
        /// <param name="element">Element</param>
        /// <param name="correctClass">Correct class</param>
        public void Teaching(List<int> element, int correctClass)
        {
            var neurousThresholdAnswers = GetNeurousThresholdAnswers(element);

            while (! IsCorrectAnswer(neurousThresholdAnswers, correctClass))
            {
                TeachNeurons(element, correctClass, neurousThresholdAnswers);
                neurousThresholdAnswers = GetNeurousThresholdAnswers(element);
            }
        }

        /// <summary>
        ///     Teach neurons
        /// </summary>
        ///
        /// <param name="element">Element</param>
        /// <param name="correctClass">Correct class</param>
        /// <param name="neurousThresholdAnswers">Neurons threshold answers</param>
        private void TeachNeurons(List<int> element, int correctClass,
            List<bool> neurousThresholdAnswers)
        {
            for (var i = 0; i < _neuronsCount; i++)
            {
                _neurons[i].Teaching(element, GetNeuronFactor(neurousThresholdAnswers[i], i, correctClass));
            }
        }

        /// <summary>
        ///     Get neurons threshold answers
        /// </summary>
        ///
        /// <param name="element">Element</param>
        ///
        /// <returns>List<bool></returns>
        private List<bool> GetNeurousThresholdAnswers(IEnumerable<int> element)
        {
            return _neurons.Select(x => x.GetAnswer(element) > _threshold).ToList();
        }

        /// <summary>
        ///     Is correct answer
        /// </summary>
        ///
        /// <param name="neurousThresholdAnswers">Neurons threshold answers</param>
        /// <param name="correctClass">Correct class</param>
        ///
        /// <returns>bool</returns>
        private bool IsCorrectAnswer(IReadOnlyList<bool> neurousThresholdAnswers, int correctClass)
        {
            return neurousThresholdAnswers[correctClass] && neurousThresholdAnswers.Count(x => x) == 1;
        }

        /// <summary>
        ///     Get neuron factor
        /// </summary>
        ///
        /// <param name="neuronThresholdAnswer">Neurons threshold answers</param>
        /// <param name="neuronNumber">Neuton number</param>
        /// <param name="correctClass">Correct class</param>
        ///
        /// <returns>int</returns>
        private int GetNeuronFactor(bool neuronThresholdAnswer, int neuronNumber, int correctClass)
        {
            if (neuronThresholdAnswer)
            {
                return neuronNumber == correctClass ? 0 : -1;
            }

            return neuronNumber == correctClass ? 1 : 0;
        }

        /// <summary>
        ///     Get answer
        /// </summary>
        ///
        /// <param name="element">Element</param>
        ///
        /// <returns>int</returns>
        public virtual int GetAnswer(List<int> element)
        {
            var neuronsAnswer = _neurons.Select(x => x.GetAnswer(element)).ToList();

            return GetMaxValueIndex(neuronsAnswer);
        }

        /// <summary>
        ///     Get max value index
        /// </summary>
        ///
        /// <param name="neuronsAnswer">Neuron answer</param>
        ///
        /// <returns>int</returns>
        private static int GetMaxValueIndex(List<int> neuronsAnswer)
        {
            var maxIndex = 0;
            for (var i = 0; i < neuronsAnswer.Count; i++)
            {
                if (neuronsAnswer[i] > neuronsAnswer[maxIndex])
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }
    }
}