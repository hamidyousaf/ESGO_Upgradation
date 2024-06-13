global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Application.DTOs.Requests;
global using Domain.CQRS.Admins;
global using Domain.DTOs.Requests;
global using Domain.Enums;
global using System.ComponentModel.DataAnnotations;
global using Domain.CQRS.Auth;
global using Domain.CQRS.Books;
global using Microsoft.AspNetCore.RateLimiting;
global using Domain.CQRS.Employees;
global using Web.Extensions;
global using Application.CQRS.Employers;
global using Application.CQRS.Timesheets;
global using System.Security.Claims;
global using Hangfire;
global using Infrastructure.DatabaseInitializers;
global using Web.Middlewares;
global using Infrastructure;
global using Microsoft.OpenApi.Models;
global using Application.CQRS.BackgroundJobs;
global using Domain.DTOs.Responces;
global using System.Net;
global using System.Text.Json;
global using Domain.CQRS.Notifications;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase {}
