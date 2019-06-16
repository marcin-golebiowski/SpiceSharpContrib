﻿using System;

namespace SpiceSharpParser.Common.Evaluation
{
    /// <summary>
    /// An evaluator expression.
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluation.Expression"/> class.
        /// </summary>
        /// <param name="expression">Expression.</param>
        public Expression(string expression)
        {
            ValueExpression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Thrown when expression is evaluated.
        /// </summary>
        public event EventHandler<EvaluatedArgs> Evaluated;

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public string ValueExpression { get; }

        /// <summary>
        /// Gets or sets the current evaluation value.
        /// </summary>
        public double CurrentValue { get; protected set; } = double.NaN;

        /// <summary>
        /// Evaluates the expression.
        /// </summary>
        /// <param name="evaluator">Evaluator.</param>
        /// <param name="context">Context.</param>
        /// <returns>
        /// The value of the expression.
        /// </returns>
        public virtual double Evaluate(IEvaluator evaluator, ExpressionContext context)
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException(nameof(evaluator));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var newValue = evaluator.EvaluateValueExpression(ValueExpression, context);
            CurrentValue = newValue;
            OnEvaluated(newValue);
            return newValue;
        }

        /// <summary>
        /// Invalidates the expression.
        /// </summary>
        public virtual void Invalidate()
        {
            CurrentValue = double.NaN;
        }

        /// <summary>
        /// Clones the expression.
        /// </summary>
        /// <returns>
        /// A cloned expression.
        /// </returns>
        public virtual Expression Clone()
        {
            return new Expression(ValueExpression);
        }

        protected void OnEvaluated(double newValue)
        {
            Evaluated?.Invoke(this, new EvaluatedArgs() { NewValue = newValue });
        }
    }
}
