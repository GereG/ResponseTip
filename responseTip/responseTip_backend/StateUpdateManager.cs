using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responseTip_backend
{
    class StateUpdateManager
    {
        private DateTime [] timeOfLastStateUpdate;
        private int[] taskStateUpdateIntervalInSeconds;
        private int[] statesValuesEnumArray;
        private int numOfStates;

        public StateUpdateManager(Type type)
        {
            Array statesNamesEnumArray = Enum.GetNames(type);
            Array newStatesValuesEnumArray = Enum.GetValues(type);
            numOfStates = newStatesValuesEnumArray.Length;

            timeOfLastStateUpdate = new DateTime[numOfStates];
            taskStateUpdateIntervalInSeconds = new int[numOfStates];
            statesValuesEnumArray = new int[numOfStates];

            for(int i=0;i<numOfStates;i++)
            {
                timeOfLastStateUpdate[i] = DateTime.MinValue;
                statesValuesEnumArray[i]=(1 << (int)newStatesValuesEnumArray.GetValue(i));

                switch ((string)statesNamesEnumArray.GetValue(i))
                {
                    case "created":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "notPaid":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "notPaid_expired":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "paid":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "questionAsked":
                        taskStateUpdateIntervalInSeconds[i] = 30;
                        break;
                    case "questionAsked_expired":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "questionAnswered":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "answerValid":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "allPaymentsSettled":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "completed":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    case "closed":
                        taskStateUpdateIntervalInSeconds[i] = 1;
                        break;
                    default:
                        throw new responseTip.Exceptions.InvalidTaskStatus();
                }
            }
        }

        public int StatesToUpdateNow()
        {
            int statesToUpdate = 0;
            TimeSpan timeDifference;
            for (int i=0;i<numOfStates;i++)
            {
                timeDifference = DateTime.Now.Subtract(timeOfLastStateUpdate[i]);
                if(timeDifference.TotalSeconds>taskStateUpdateIntervalInSeconds[i])
                {
                    timeOfLastStateUpdate[i] = DateTime.Now;
                    statesToUpdate += (int)statesValuesEnumArray.GetValue(i);
                }
            }

            return statesToUpdate;
        }
    }
}
