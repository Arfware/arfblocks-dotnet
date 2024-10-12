global using System;
global using System.Text;
global using System.Collections.Generic;
global using System.Linq;

global using Microsoft.EntityFrameworkCore;
global using FluentValidation;

global using Arfware.ArfBlocks.Core;
global using Arfware.ArfBlocks.Core.Abstractions;
global using Arfware.ArfBlocks.Test;
global using Arfware.ArfBlocks.Test.Abstractions;
global using Arfware.ArfBlocks.Core.RequestResults;
global using Arfware.ArfBlocks.Core.Attributes;
global using Arfware.ArfBlocks.Core.Exceptions;

global using TodoApp.Domain;
global using TodoApp.Domain.Entities;
global using TodoApp.Domain.BusinessRules;
global using TodoApp.Infrastructure.RelationalDB;
global using TodoApp.Infrastructure.Services;
global using BusinessModules.Management.Infrastructure.Services;