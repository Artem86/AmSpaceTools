﻿using AmSpaceModels.Idp;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmSpaceTools.ModelConverters
{
    public class ActionToUpdateConverter : ITypeConverter<CompetencyAction, UpdateAction>
    {
        public UpdateAction Convert(CompetencyAction source, UpdateAction destination, ResolutionContext context)
        {
            var result = new UpdateAction();
            result.FeedbackActions = new List<IdpAction>();
            result.PracticeActions = new List<IdpAction>();
            result.TheoryActions = new List<IdpAction>();
            foreach (var competencyAction in source.Actions)
            {
                switch (competencyAction.ActionType.Value)
                {
                    case 0:
                        {
                            result.TheoryActions.Add(competencyAction);
                            break;
                        }
                    case 1:
                        {
                            result.FeedbackActions.Add(competencyAction);
                            break;
                        }
                    case 2:
                        {
                            result.PracticeActions.Add(competencyAction);
                            break;
                        }
                }
            }
            return result;
        }
    }
}
