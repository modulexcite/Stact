// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.RulesEngine.ExecutionModel
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// An ActionNode contains an action that should be invoked when the node is activated.
	/// 
	/// This version invokes a simple action that accepts no arguments or only the base RuleContext
	/// without a generic specialization on the argument
	/// </summary>
	public class ActionNode :
		Activation,
		ModelVisitorSite
	{
		private readonly Action<RuleContext> _eval;
		private readonly Expression<Action<RuleContext>> _expression;

		public ActionNode(Expression<Action> expression)
			: this(expression.WrapActionWithArgument<RuleContext>())
		{
		}

		public ActionNode(Expression<Action<RuleContext>> expression)
		{
			Priority = 0;
			_expression = expression;

			_eval = CompileExpression(_expression);
		}

		public int Priority { get; private set; }

		public Expression Expression
		{
			get { return _expression; }
		}

		public void Activate<T>(RuleContext<T> context)
		{
			context.EnqueueAgendaAction(Priority, () => _eval(context));
		}

		public bool Visit(ModelVisitor visitor)
		{
			return visitor.Visit(this);
		}

		private static Action<RuleContext> CompileExpression(Expression<Action<RuleContext>> expression)
		{
			try
			{
				return expression.Compile();
			}
			catch (Exception ex)
			{
				throw new CompileExpressionException(expression, ex);
			}
		}
	}

	/// <summary>
	/// An ActionNode contains an action that should be invoked when the node is activated.
	/// 
	/// This version invokes a simple action that accepts no arguments or only the base RuleContext
	/// without a generic specialization on the argument
	/// </summary>
	public class ActionNode<T> :
		Node,
		Activation<T>,
		ModelVisitorSite
	{
		private readonly Action<RuleContext<T>> _eval;
		private readonly Expression<Action<RuleContext<T>>> _expression;

		public ActionNode(Expression<Action<RuleContext<T>>> expression)
		{
			Priority = 0;

			_expression = expression;

			_eval = CompileExpression(_expression);
		}

		public Expression Expression
		{
			get { return _expression; }
		}

		public int Priority { get; private set; }

		public void Activate(RuleContext<T> context)
		{
			context.EnqueueAgendaAction(Priority, () => _eval(context));
		}

		private static Action<RuleContext<T>> CompileExpression(Expression<Action<RuleContext<T>>> expression)
		{
			try
			{
				return expression.Compile();
			}
			catch (Exception ex)
			{
				throw new CompileExpressionException(expression, ex);
			}
		}

		public bool Visit(ModelVisitor visitor)
		{
			return visitor.Visit(this);
		}
	}
}