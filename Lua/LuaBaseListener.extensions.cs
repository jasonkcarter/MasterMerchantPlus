namespace Lua
{
    public partial class LuaBaseListener
    {
        /// <summary>
        ///     Gets a name/value pair representing a given Lua table field and whether the value is a nested table or not.
        /// </summary>
        /// <param name="context">The parse tree node for the field.</param>
        /// <param name="valueChild">
        ///     (optional) The parse tree node for the value, if known; if not known, it is assumed to be the
        ///     fifth child of the field node.
        /// </param>
        /// <returns></returns>
        protected LuaTableField GetField(LuaParser.FieldContext context, LuaParser.ExpContext valueChild = null)
        {
            // Fields are expected to have 5 children: <left bracket> <name> <right bracket> <equal> <value>
            // When called from the table constructor enter method, the <value> child is not yet populated, so we pass it explicitly.
            int expectedChildCount = valueChild == null ? 5 : 4;

            // Validate field count
            if (context.ChildCount < expectedChildCount) return null;

            // Value child not specified, so get it from the fifth child.
            if (valueChild == null)
            {
                valueChild = context.GetChild(4) as LuaParser.ExpContext;
            }

            // Value isn't an expression? Malformed Lua table.
            if (valueChild == null) return null;

            // Second child is the <name> node.
            var keyChild = context.GetChild(1) as LuaParser.ExpContext;

            // Name wasn't an expression? Malformed Lua table. 
            if (keyChild == null) return null;

            // Create key/value instance and return
            var field = new LuaTableField
            {
                Name = GetString(keyChild),
                IsTable = valueChild.Start.Type == LuaParser.T__48 // left curly brace token indicates a nested table
            };
            if (!field.IsTable)
            {
                field.Value = GetString(valueChild);
            }
            return field;
        }

        protected LuaTableField GetParentField(LuaParser.TableconstructorContext context)
        {
            if (!(context.Parent is LuaParser.ExpContext)
                || !(context.Parent.Parent is LuaParser.FieldContext))
            {
                return null;
            }
            return GetField((LuaParser.FieldContext) context.Parent.Parent,
                (LuaParser.ExpContext) context.Parent);
        }

        /// <summary>
        ///     Gets the given expression as a string value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The string value of the expression.</returns>
        protected static string GetString(LuaParser.ExpContext context)
        {
            string text = context.GetText();
            if (context.Start.Type == LuaParser.NORMALSTRING
                && context.Stop.Type == LuaParser.NORMALSTRING
                && text.Length > 1)
            {
                text = text.Substring(1, text.Length - 2);
            }
            return text;
        }
    }
}