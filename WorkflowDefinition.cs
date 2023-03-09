using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Databases.SqlServer.Context;
using Bpms.WorkflowEngine.Enums;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Bpms.WorkflowEngine.Models;
using Task = Bpms.WorkflowEngine.Databases.Entities.Task;

namespace Bpms.WorkflowEngine
{
    //Impelementing Read xpdl,define performers and so on

    public class WorkflowDefinition : IWorkflowDefinition
    {
        private readonly IBpmsUnitOfWork _bpmsUnitOfWork;

        public WorkflowDefinition(IBpmsUnitOfWork bpmsUnitOfWork)
        {
            _bpmsUnitOfWork = bpmsUnitOfWork;
        }


        public async Task<Activity> GetActivity(int activityId)
        {
            return await _bpmsUnitOfWork.Activities.FindAsync(activityId);
        }

        public async Task<RuntimeActivity> GetActivityByTaskId(int taskId)
        {
            var activity = await (from t in _bpmsUnitOfWork.Tasks
                                  join ra in _bpmsUnitOfWork.RuntimeActivities
                                      on t.RuntimeActivityId equals ra.Id
                                  where taskId == t.Id
                                  select ra).FirstAsync();

            return activity;
        }

        public async Task<List<Workflow>> GetWorkflows()
        {
            return await _bpmsUnitOfWork.Workflows.ToListAsync();
        }

        public async Task<Workflow> ImportXpdl(string path,
            Workflow workflow)
        {
            var xdoc = XDocument.Load(path);
            var ns = xdoc.Root.Name.Namespace;

            //var workflow = new Workflow
            //{
            //    Guid = guid,// new Guid(xdoc.Root.Attribute("Id")?.Value),
            //    Title = title,// xdoc.Root.Attribute("Name")?.Value,
            //    CreatedBy = createdBy,
            //    CreateTime = createTime,
            //    UniqueName = uniqueName,
            //    Description = description,
            //    WorkflowName = workflowName,
            //    IsActive = isActive,
            //    DocumentId = documentId
            //};
            var insertedWorkFlow = _bpmsUnitOfWork.Workflows.Add(workflow);

            foreach (var xPool in xdoc.Root.Descendants(ns + "Pool"))
            {
                var pool = new Pool
                {
                    Guid = new Guid(xPool.Attribute("Id").Value),
                    Name = xPool.Attribute("Name").Value,
                    Workflow = insertedWorkFlow.Entity,
                    BoundaryVisible = Convert.ToBoolean(xPool.Attribute("BoundaryVisible").Value)
                };

                foreach (var xNodeGraphicInfo in xPool.Descendants(ns + "NodeGraphicsInfo"))
                {
                    pool.Shape = new Shape
                    {
                        BorderColor = (xNodeGraphicInfo.Attribute("BorderColor") != null)
                            ? (Convert.ToInt32(xNodeGraphicInfo.Attribute("BorderColor").Value))
                            : 0,
                        Expanded = (xNodeGraphicInfo.Attribute("Expanded") != null) &&
                                   (Convert.ToBoolean(xNodeGraphicInfo.Attribute("Expanded").Value)),
                        ExpandedHeight = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                            ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedHeight").Value))
                            : 0,
                        ExpandedWidth = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                            ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedWidth").Value))
                            : 0,
                        FillColor = (xNodeGraphicInfo.Attribute("FillColor") != null)
                            ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("FillColor").Value))
                            : 0,
                        Height = (xNodeGraphicInfo.Attribute("Height") != null)
                            ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Height").Value))
                            : 0,
                        Width = (xNodeGraphicInfo.Attribute("Width") != null)
                            ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Width").Value))
                            : 0
                    };

                    foreach (var xCoordinate in xNodeGraphicInfo.Descendants(ns + "Coordinates"))
                    {
                        pool.Shape.XCoordinate = ((int) Convert.ToDouble(xCoordinate.Attribute("XCoordinate").Value));
                        pool.Shape.YCoordinate = ((int) Convert.ToDouble(xCoordinate.Attribute("YCoordinate").Value));
                    }
                }

                var insertedPool = _bpmsUnitOfWork.Pools.Add(pool);

                foreach (var xLane in xPool.Descendants(ns + "Lane"))
                {
                    var lane = new Lane
                    {
                        Guid = new Guid(xLane.Attribute("Id").Value),
                        Pool = insertedPool.Entity,
                        Name = xLane.Attribute("Name").Value,
                    };
                    foreach (var xNodeGraphicInfo in xLane.Descendants(ns + "NodeGraphicsInfo"))
                    {
                        lane.Shape = new Shape
                        {
                            BorderColor = (xNodeGraphicInfo.Attribute("BorderColor") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("BorderColor").Value))
                                : 0,
                            Expanded = (xNodeGraphicInfo.Attribute("Expanded") != null) &&
                                       (Convert.ToBoolean(xNodeGraphicInfo.Attribute("Expanded").Value)),
                            ExpandedHeight = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedHeight").Value))
                                : 0,
                            ExpandedWidth = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedWidth").Value))
                                : 0,
                            FillColor = (xNodeGraphicInfo.Attribute("FillColor") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("FillColor").Value))
                                : 0,
                            Height = (xNodeGraphicInfo.Attribute("Height") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Height").Value))
                                : 0,
                            Width = (xNodeGraphicInfo.Attribute("Width") != null)
                                ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Width").Value))
                                : 0
                        };

                        foreach (var xCoordinate in xNodeGraphicInfo.Descendants(ns + "Coordinates"))
                        {
                            lane.Shape.XCoordinate =
                                ((int) Convert.ToDouble(xCoordinate.Attribute("XCoordinate").Value));
                            lane.Shape.YCoordinate =
                                ((int) Convert.ToDouble(xCoordinate.Attribute("YCoordinate").Value));
                        }

                        _bpmsUnitOfWork.Lanes.Add(lane);
                    }
                }

                ;
            }

            foreach (var xProcess in xdoc.Root.Descendants(ns + "WorkflowProcess"))
            {
                if (xProcess.Descendants(ns + "Activity").Count() != 0)
                {
                    var insertedProcess = _bpmsUnitOfWork.Processes.Add(new Process
                    {
                        Guid = new Guid(xProcess.Attribute("Id").Value),
                        Name = xProcess.Attribute("Name").Value,
                        Workflow = insertedWorkFlow.Entity,
                        IsActivitySet = false
                    });

                    foreach (var xActivity in xProcess.Descendants(ns + "Activity"))
                    {
                        var activity = new Activity
                        {
                            Guid = new Guid(xActivity.Attribute("Id").Value),
                            Process = insertedProcess.Entity,
                            Name = xActivity.Attribute("Name").Value,
                            Type = (int) ActivityTypes.Activity
                        };

                        foreach (var item in xActivity.Descendants(ns + "Route"))
                        {
                            var gatewayType = item.Attribute("GatewayType") == null
                                ? ""
                                : item.Attribute("GatewayType").Value;
                            if (gatewayType == "")
                                activity.Type = (int) ActivityTypes.NormalGateway;
                            else if (gatewayType == "Inclusive")
                                activity.Type = (int) ActivityTypes.InclusiveGateWay;
                            else if (gatewayType == "Parallel")
                                activity.Type = (int) ActivityTypes.ParallelGateWay;

                        }

                        foreach (var xNodeGraphicInfo in xActivity.Descendants(ns + "NodeGraphicsInfo"))
                        {
                            activity.Shape = new Shape
                            {
                                BorderColor = (xNodeGraphicInfo.Attribute("BorderColor") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("BorderColor").Value))
                                    : 0,
                                Expanded = (xNodeGraphicInfo.Attribute("Expanded") != null) &&
                                           (Convert.ToBoolean(xNodeGraphicInfo.Attribute("Expanded").Value)),
                                ExpandedHeight = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                    ? (int) (Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedHeight").Value))
                                    : 0,
                                ExpandedWidth = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedWidth").Value))
                                    : 0,
                                FillColor = (xNodeGraphicInfo.Attribute("FillColor") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("FillColor").Value))
                                    : 0,
                                Height = (xNodeGraphicInfo.Attribute("Height") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Height").Value))
                                    : 0,
                                Width = (xNodeGraphicInfo.Attribute("Width") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Width").Value))
                                    : 0
                            };
                            //type
                            // 0 :startevent
                            foreach (var xCoordinate in xNodeGraphicInfo.Descendants(ns + "Coordinates"))
                            {
                                activity.Shape.XCoordinate =
                                    ((int) Convert.ToDouble(xCoordinate.Attribute("XCoordinate").Value));
                                activity.Shape.YCoordinate =
                                    ((int) Convert.ToDouble(xCoordinate.Attribute("YCoordinate").Value));
                            }
                        }

                        var insertedActivity = _bpmsUnitOfWork.Activities.Add(activity);

                        foreach (var xPerformers in xActivity.Descendants(ns + "Performer"))
                        {
                            _bpmsUnitOfWork.Performers.Add(new Performer
                            {
                                Activity = insertedActivity.Entity,
                            });
                        }

                        foreach (var xStartEvent in xActivity.Descendants(ns + "StartEvent"))
                        {
                            insertedActivity.Entity.Type = (int) ActivityTypes.StartEvent;
                            var newEvent = new Event
                            {
                                Activity = insertedActivity.Entity,
                                Trigger = (xStartEvent.Attribute("Trigger") == null)
                                    ? ""
                                    : xStartEvent.Attribute("Trigger").Value,
                                Type = (int) EventTypes.Start, //StartEvent

                            };
                            _bpmsUnitOfWork.Events.Add(newEvent);
                        }

                        foreach (var xEndEvent in xActivity.Descendants(ns + "EndEvent"))
                        {
                            insertedActivity.Entity.Type = (int) ActivityTypes.EndEvent;
                            _bpmsUnitOfWork.Events.Add(new Event
                            {
                                Activity = insertedActivity.Entity,
                                Trigger = (xEndEvent.Attribute("Trigger") == null)
                                    ? ""
                                    : xEndEvent.Attribute("Trigger").Value,
                                Type = (int) EventTypes.End, //EndEvent
                            });
                        }

                        foreach (var xIntermediateEvent in xActivity.Descendants(ns + "IntermediateEvent"))
                        {
                            activity.Type = (int) ActivityTypes.IntermmediateEvent;
                            _bpmsUnitOfWork.Events.Add(new Event
                            {
                                Activity = insertedActivity.Entity,
                                IsAttached = Convert.ToBoolean(xIntermediateEvent.Attribute("IsAttached").Value),
                                Target = new Guid(xIntermediateEvent.Attribute("Target").Value),
                                Trigger = (xIntermediateEvent.Attribute("Trigger") == null)
                                    ? ""
                                    : xIntermediateEvent.Attribute("Trigger").Value,
                                Type = (int) EventTypes.Intermmediate, //IntermidiateEvent
                            });
                        }
                    }

                    foreach (var xTransition in xProcess.Descendants(ns + "Transition"))
                    {
                        var indertedTransition = _bpmsUnitOfWork.Transitions.Add(new Transition
                        {
                            Guid = new Guid(xTransition.Attribute("Id").Value),
                            Process = insertedProcess.Entity,
                            Name = (xTransition.Attribute("Name") == null) ? "" : xTransition.Attribute("Name").Value,

                            To = new Guid(xTransition.Attribute("To").Value),
                            From = new Guid(xTransition.Attribute("From").Value),
                            Condition = (xTransition.Attribute("Condition") == null)
                                ? ""
                                : xTransition.Attribute("Condition").Value,
                        });


                        foreach (var xNodeGraphicInfo in xTransition.Descendants(ns + "ConnectorGraphicsInfo"))
                        {
                            foreach (var xCoordinate in xNodeGraphicInfo.Descendants(ns + "Coordinates"))
                            {
                                var insertedShape = _bpmsUnitOfWork.Shapes.Add(new Shape()
                                {
                                    BorderColor = (xNodeGraphicInfo.Attribute("BorderColor") != null)
                                        ? (Convert.ToInt32(xNodeGraphicInfo.Attribute("BorderColor").Value))
                                        : 0,
                                    XCoordinate =
                                        ((int) Convert.ToDouble(
                                            Convert.ToDouble(xCoordinate.Attribute("XCoordinate").Value))),
                                    YCoordinate =
                                        ((int) Convert.ToDouble(
                                            Convert.ToDouble(xCoordinate.Attribute("YCoordinate").Value))),
                                });

                                _bpmsUnitOfWork.TransitionShapes.Add(new TransitionShape()
                                {
                                    Transition = indertedTransition.Entity,
                                    Shape = insertedShape.Entity
                                });
                            }
                        }
                    }


                    foreach (var xActivitySet in xProcess.Descendants(ns + "ActivitySet"))
                    {
                        var activitySetProcess = new Process
                        {
                            Guid = new Guid(xActivitySet.Attribute("Id").Value),
                            Name = (xActivitySet.Attribute("Name") == null) ? "" : xActivitySet.Attribute("Name").Value,
                            Workflow = insertedWorkFlow.Entity,
                            IsActivitySet = true,
                        };
                        foreach (var xNodeGraphicInfo in xActivitySet.Descendants(ns + "NodeGraphicsInfo"))
                        {
                            activitySetProcess.Shape = new Shape
                            {
                                BorderColor = (xNodeGraphicInfo.Attribute("BorderColor") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("BorderColor").Value))
                                    : 0,
                                Expanded = (xNodeGraphicInfo.Attribute("Expanded") != null) &&
                                           (Convert.ToBoolean(xNodeGraphicInfo.Attribute("Expanded").Value)),
                                ExpandedHeight = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedHeight").Value))
                                    : 0,
                                ExpandedWidth = (xNodeGraphicInfo.Attribute("ExpandedHeight") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("ExpandedWidth").Value))
                                    : 0,
                                FillColor = (xNodeGraphicInfo.Attribute("FillColor") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("FillColor").Value))
                                    : 0,
                                Height = (xNodeGraphicInfo.Attribute("Height") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Height").Value))
                                    : 0,
                                Width = (xNodeGraphicInfo.Attribute("Width") != null)
                                    ? ((int) Convert.ToDouble(xNodeGraphicInfo.Attribute("Width").Value))
                                    : 0
                            };

                            foreach (var xCoordinate in xNodeGraphicInfo.Descendants(ns + "Coordinates"))
                            {
                                activitySetProcess.Shape.XCoordinate =
                                    ((int) Convert.ToDouble(
                                        Convert.ToDouble(xCoordinate.Attribute("XCoordinate").Value)));
                                activitySetProcess.Shape.YCoordinate =
                                    ((int) Convert.ToDouble(
                                        Convert.ToDouble(xCoordinate.Attribute("YCoordinate").Value)));
                            }
                        }

                        _bpmsUnitOfWork.Processes.Add(activitySetProcess);
                    }
                }
            }

            ;
            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
            return insertedWorkFlow.Entity;
        }

        public async Task<bool> AddPerformers(List<PossiblePerformerModel> performerList)
        {
            if (performerList == null) return false;
            var activityId = performerList[0].ActivityId;
            if (activityId == null) return true;
            var performer = new Performer
            {
                ActivityId = (int)activityId,
            };
            foreach (var item in performerList)
            {
                if (item.ParentNodeId != 0) continue;
                var combination = new PerformerCondition
                {
                    Performer = performer,
                    IsEqual = item.IsEqual,
                    PerformerType = ((PerformerCondition)item).PerformerType,
                    ValueSourceType = item.PerformerType,
                    NodeType = item.NodeType,
                    ParentId = item.ParentId
                };
                foreach (var condition in performerList
                    .Where(condition => condition.ParentNodeId == item.NodeId))
                {
                }
                performer.PerformerConditions.Add(combination);
            }

            _bpmsUnitOfWork.Performers.Add(performer);

            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);

            return true;
        }

        public async Task<bool> UpdateActivity(Activity activity)
        {
            _bpmsUnitOfWork.Activities.Update(activity);
            await _bpmsUnitOfWork.SaveAsync(CancellationToken.None);
            return true;
        }

        public async Task<int> GetActivityState(int taskId, int activityId)
        {
            var color = -1249281;// رنگ آبی رنگ دیفالت اکتویتیها

            var task = await _bpmsUnitOfWork.Tasks.Where(c => c.Id == taskId).FirstOrDefaultAsync();
            var query = await (from t in _bpmsUnitOfWork.Tasks
                    .Where(x => x.RuntimeWorkflowId == task.RuntimeWorkflowId)
                               join ra in _bpmsUnitOfWork.RuntimeActivities
                                   .Where(x => x.ActivityId == activityId) on t
                                   .RuntimeActivityId equals ra.Id
                               select new
                               {
                                   EndTime = t.EndTime,
                               }).FirstOrDefaultAsync();

            if (query != null)
            {
                if (query.EndTime != null)
                    color = -554455;// رنگ نارنجی برای اکتیویتی های انجام  شده
                else
                    color = -5125551;// رنگ سبز برای اکتیویتی جاری
            }
            if (activityId == 545)// اکتیویتی ثبت درخواست که برایش تسک تعریف نشده و استارت کار میباشد
                color = -554455;// به رنگ بقیه تسکهای انجام شده در می آید
            return color;
        }

        public async Task<List<GraphicShape>> GetShapesForGraphicalWorkflow(int workflowId, int taskId)
        {
            var lanes = await (from l in _bpmsUnitOfWork.Lanes.Where(c => c.Pool.WorkflowId == workflowId)
                               select new GraphicShape
                               {
                                   BorderColor = l.Shape.BorderColor,
                                   Expanded = l.Shape.Expanded,
                                   ExpandedHeight = l.Shape.ExpandedHeight,
                                   ExpandedWidth = l.Shape.ExpandedWidth,
                                   FillColor = l.Shape.FillColor,
                                   Height = l.Shape.Height,
                                   Id = l.Id,
                                   Width = l.Shape.Width,
                                   Name = l.Name,
                                   ShapeType = 1,//rectangle
                                   XCoordinate = l.Shape.XCoordinate,
                                   YCoordinate = l.Shape.YCoordinate + 30,//////////////////////////////////////// تغییر مختصات
                                   Type = "Lane"
                               }).ToListAsync();

            if (taskId != 0)
            {
                var id = taskId;
                taskId = await (from rp in _bpmsUnitOfWork.RuntimeProcesses
                                join ra in _bpmsUnitOfWork.RuntimeActivities on rp.Id equals ra.RuntimeProcessId
                                join t in _bpmsUnitOfWork.Tasks on ra.Id equals t.RuntimeActivityId
                                where t.Id == id
                                select rp.Id).FirstAsync();
            }

            var activities = await (from a in _bpmsUnitOfWork.Activities.Where(c => c.Process.WorkflowId == workflowId)
                                    join raL in _bpmsUnitOfWork.RuntimeActivities.Where(x => x.RuntimeProcessId == taskId) on a.Id equals raL.ActivityId into raNull
                                    from ra in raNull.DefaultIfEmpty()
                                    select new GraphicShape
                                    {
                                        BorderColor = a.Shape.BorderColor,
                                        Expanded = a.Shape.Expanded,
                                        ExpandedHeight = a.Shape.ExpandedHeight,
                                        ExpandedWidth = a.Shape.ExpandedWidth,
                                        FillColor = ra == null ? a.Shape.FillColor : (ra.EndDate != null ? -554455 : -5125551),
                                        Height = a.Shape.Height,
                                        Id = a.Id,
                                        Width = a.Shape.Width,
                                        Name = a.Name,
                                        ShapeType = (int)(a.Type),//(a.Type == null) ? (int)(a.Type) : 1,
                                        XCoordinate = a.Shape.XCoordinate - 30,//////////////////////////////////////// تغییر مختصات
                                        YCoordinate = a.Shape.YCoordinate,
                                        Type = "Activity"
                                    }).ToListAsync();


            var shapes = lanes.ToList();
            shapes.AddRange(activities);

            return shapes;
        }

        public async Task<List<Transition>> GetTransitions(int workflowId)
        {
            return await _bpmsUnitOfWork.Transitions
                .Where(c => c.Process.WorkflowId == workflowId)
                .ToListAsync();
        }
        public async Task<List<Shape>> GetTransitionCoordinates(int workflowId)
        {
            var coordinates = await _bpmsUnitOfWork.TransitionShapes
                .Where(c => c.Transition.Process.WorkflowId == workflowId).Select(x => x.Shape)
                .ToListAsync();

            foreach (var item in coordinates)
            {
                item.XCoordinate -= 30;//////////////////////////////////////// تغییر مختصات
            }

            return coordinates;
        }

        public Task<List<PossiblePerformerModel>> GetPossiblePerformers()
        {
            throw new NotImplementedException();
        }

        public async Task<List<PossiblePerformerModel>> GetPossiblePerformers(ICollection<Expression<Func<PossiblePerformerModel, bool>>> possiblePerformerModels)
        {
            return null;
            /*  var units = await _bpmsUnitOfWork.Units.ToListAsync();
              var positions = await _bpmsUnitOfWork.Positions.ToListAsync();
              var roles = await _bpmsUnitOfWork.Roles.ToListAsync();
              var users = await (from u in _bpmsUnitOfWork.Users
                                 join p in _bpmsUnitOfWork.Persons on u.PersonId equals p.Id
                                 join e in _bpmsUnitOfWork.Employees on p.Id equals e.PersonId
                                 select new { Text = p.LastName + " " + p.FirstName, Id = u.Id })
                  .ToListAsync();

              var possiblePerformers = units
                  .Select(item => new PossiblePerformerModel
                  {
                      PossiblePerformerModelId = item.Id.ToString(),
                      Text = item.ToString(),
                      FieldName = "Unit"
                  }).ToList();

              possiblePerformers.AddRange(positions
                  .Select(item => new PossiblePerformerModel
                  {
                      PossiblePerformerModelId = item.Id.ToString(),
                      Text = item.Title,
                      FieldName = "Position"
                  }));

              possiblePerformers.AddRange(roles
                  .Select(item => new PossiblePerformerModel
                  {
                      PossiblePerformerModelId = item.Id.ToString(),
                      Text = item.Title,
                      FieldName = "Role"
                  }));

              possiblePerformers.AddRange(users
                  .Select(item => new PossiblePerformerModel
                  {
                      PossiblePerformerModelId = item.Id.ToString(),
                      Text = item.Text,
                      FieldName = "User"
                  }));

              return possiblePerformers;*/
        }
        public async Task<List<int>> GetTransitionIds(int workflowId)
        {
            return await _bpmsUnitOfWork.TransitionShapes
                .Where(c => c.Transition.Process.WorkflowId == workflowId)
                .Select(c => c.Transition.Id).ToListAsync();
        }
        public async Task<List<int?>> GetCoordinatesArray(int transitionId)
        {
            var coordinatesArray = new List<int?>();

            var xs = await _bpmsUnitOfWork.TransitionShapes
                .Where(x => x.Transition.Id == transitionId)
                .Select(x => x.Shape.XCoordinate)
                .ToListAsync();



            var ys = await _bpmsUnitOfWork.TransitionShapes
                .Where(x => x.Transition.Id == transitionId)
                .Select(x => x.Shape.YCoordinate)
                .ToListAsync();

            for (var i = 0; i < xs.Count; i++)
            {
                coordinatesArray.Add(xs[i]);
                coordinatesArray.Add(ys[i]);
            }
            return coordinatesArray;
        }
        public async Task<List<TransitionLine>> GetTransitionLines(int workflowId)
        {
            var transitionLines = new List<TransitionLine>();
            var transitions = await _bpmsUnitOfWork.Transitions
                .Include(x => x.TransitionShapes).ThenInclude(x => x.Shape)
                .Where(c => c.Process.WorkflowId == workflowId)
                .ToListAsync();

            var coordinates = await _bpmsUnitOfWork.TransitionShapes
                .Where(c => c.Transition.Process.WorkflowId == workflowId)
                .ToListAsync();

            foreach (var item in coordinates)
            {
                item.Shape.XCoordinate -= 30;//////////////////////////////////////// تغییر مختصات
            }

            foreach (var tran in transitions)
            {

                var tranCoordinateList = tran.TransitionShapes.Select(x => x.Shape).ToList();
                var lineCount = tranCoordinateList.Count - 1;
                for (var i = 0; i < lineCount; i += 1)
                {
                    var line = new TransitionLine
                    {
                        TransitionId = tran.Id,
                        BorderColor = tranCoordinateList[i].BorderColor,
                        XSource = tranCoordinateList[i].XCoordinate ?? 0,
                        YSource = tranCoordinateList[i].YCoordinate ?? 0,
                        SourceCoordinateId = tranCoordinateList[i].Id,
                        XDestination = tranCoordinateList[i + 1].XCoordinate ?? 0,
                        YDestination = tranCoordinateList[i + 1].YCoordinate ?? 0,
                        DestinationCoordinateId = tranCoordinateList[i + 1].Id,
                        IsFinalLineForTran = false
                    };
                    if (i == 0)
                        line.IsFirstLineForTran = true;
                    if (i == lineCount - 1)
                        line.IsFinalLineForTran = true;
                    line.Direct = "";//عمودی رو به بالا:up  right:افقی رو به راست  down:عمودی رو به پایین left:افقی رو به چپ
                    if (line.XDestination == line.XSource)//عمودی
                        line.Direct = line.YDestination > line.YSource ? "down" : "up";
                    if (line.YDestination == line.YSource)//افقی
                        line.Direct = line.XDestination > line.XSource ? "right" : "left";
                    line.Name = tran.Name;
                    transitionLines.Add(line);
                }

            }
            return transitionLines;

        }
    }


}
