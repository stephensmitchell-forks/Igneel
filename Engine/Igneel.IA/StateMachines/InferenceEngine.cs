﻿using Igneel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.IA.StateMachines
{
    public class InferenceEngine:Automata
    {
        ObservedDictionary<string, Fact> _facts;
        List<Action<InferenceEngine>> _sensors = new List<Action<InferenceEngine>>();

        public event Action<Fact> FactAdded;
        public event Action<Fact> FactRemoved;

        public InferenceEngine()
        {
            _facts = new ObservedDictionary<string, Fact>(x => OnFactAdded(x), x => OnFactRemoved(x), x => x.Name);
        }

        public ObservedDictionary<string, Fact> Facts { get { return _facts; } }

        public List<Action<InferenceEngine>> Sensors { get { return _sensors; } }

        public override void Update(float elapsedTime)
        {
            //gather world informations from the sensors and updates the facts
            foreach (var sensor in _sensors)
            {
                sensor(this);
            }        
            //update the states
            base.Update(elapsedTime);
        }

        private void OnFactAdded(Fact item)
        {
            if (FactAdded != null)
                FactAdded(item);
        }
        private void OnFactRemoved(Fact item)
        {
            if (FactRemoved != null)
                FactRemoved(item);
        }
    }
}
