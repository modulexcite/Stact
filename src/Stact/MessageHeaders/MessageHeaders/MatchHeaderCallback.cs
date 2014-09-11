﻿// Copyright 2010 Chris Patterson
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
namespace Stact.MessageHeaders
{
	public interface MatchHeaderCallback
	{
		void Body<TBody>(TBody body);
		void Message<TBody>(Message<TBody> message);
		void Request<TRequest>(Request<TRequest> request);
		void Response<TResponse>(Response<TResponse> response);
	}


	/// <summary>
	/// Matches the header type and carries along a context value with the match
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public interface MatchHeaderCallback<in TContext>
	{
		void Body<TBody>(TContext context, TBody body);
		void Message<TBody>(TContext context, Message<TBody> message);
		void Request<TRequest>(TContext context, Request<TRequest> request);
		void Response<TResponse>(TContext context, Response<TResponse> response);
	}
}