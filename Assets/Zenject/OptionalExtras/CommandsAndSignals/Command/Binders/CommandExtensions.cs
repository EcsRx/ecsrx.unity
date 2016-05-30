using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;

namespace Zenject
{
    public static class CommandExtensions
    {
        // Zero parameters
        public static CommandBinder<TCommand> BindCommand<TCommand>(this DiContainer container, string identifier)
            where TCommand : Command
        {
            return new CommandBinder<TCommand>(identifier, container);
        }

        public static CommandBinder<TCommand> BindCommand<TCommand>(this DiContainer container)
            where TCommand : Command
        {
            return BindCommand<TCommand>(container, null);
        }

        // One parameter
        public static CommandBinder<TCommand, TParam1> BindCommand<TCommand, TParam1>(this DiContainer container, string identifier)
            where TCommand : Command<TParam1>
        {
            return new CommandBinder<TCommand, TParam1>(identifier, container);
        }

        public static CommandBinder<TCommand, TParam1> BindCommand<TCommand, TParam1>(this DiContainer container)
            where TCommand : Command<TParam1>
        {
            return BindCommand<TCommand, TParam1>(container, null);
        }

        // Two parameters

        public static CommandBinder<TCommand, TParam1, TParam2> BindCommand<TCommand, TParam1, TParam2>(this DiContainer container, string identifier)
            where TCommand : Command<TParam1, TParam2>
        {
            return new CommandBinder<TCommand, TParam1, TParam2>(identifier, container);
        }

        public static CommandBinder<TCommand, TParam1, TParam2> BindCommand<TCommand, TParam1, TParam2>(this DiContainer container)
            where TCommand : Command<TParam1, TParam2>
        {
            return BindCommand<TCommand, TParam1, TParam2>(container, null);
        }

        // Three parameters
        public static CommandBinder<TCommand, TParam1, TParam2, TParam3> BindCommand<TCommand, TParam1, TParam2, TParam3>(this DiContainer container, string identifier)
            where TCommand : Command<TParam1, TParam2, TParam3>
        {
            return new CommandBinder<TCommand, TParam1, TParam2, TParam3>(identifier, container);
        }

        public static CommandBinder<TCommand, TParam1, TParam2, TParam3> BindCommand<TCommand, TParam1, TParam2, TParam3>(this DiContainer container)
            where TCommand : Command<TParam1, TParam2, TParam3>
        {
            return BindCommand<TCommand, TParam1, TParam2, TParam3>(container, null);
        }

        // Four parameters
        public static CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4> BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4>(this DiContainer container, string identifier)
            where TCommand : Command<TParam1, TParam2, TParam3, TParam4>
        {
            return new CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4>(identifier, container);
        }

        public static CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4> BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4>(this DiContainer container)
            where TCommand : Command<TParam1, TParam2, TParam3, TParam4>
        {
            return BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4>(container, null);
        }

        // Five parameters
        public static CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5> BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5>(this DiContainer container, string identifier)
            where TCommand : Command<TParam1, TParam2, TParam3, TParam4, TParam5>
        {
            return new CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5>(identifier, container);
        }

        public static CommandBinder<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5> BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5>(this DiContainer container)
            where TCommand : Command<TParam1, TParam2, TParam3, TParam4, TParam5>
        {
            return BindCommand<TCommand, TParam1, TParam2, TParam3, TParam4, TParam5>(container, null);
        }
    }
}

