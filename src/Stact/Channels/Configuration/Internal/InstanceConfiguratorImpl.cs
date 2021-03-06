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
namespace Stact.Configuration.Internal
{
	using System;
	using Builders;


	public class InstanceConfiguratorImpl<TChannel> :
		InstanceConfigurator<TChannel>,
		ChannelBuilderConfigurator<TChannel>
	{
		ChannelBuilderConfigurator<TChannel> _configurator;

		public void Configure(ChannelBuilder<TChannel> builder)
		{
			_configurator.Configure(builder);
		}

		public void ValidateConfiguration()
		{
			if (_configurator == null)
				throw new ChannelConfigurationException(typeof(TChannel), "No channel configurator was setup");

			_configurator.ValidateConfiguration();
		}

		public InstanceConfigurator<TInstance, TChannel> Of<TInstance>()
			where TInstance : class
		{
			var configurator = new InstanceConfiguratorImpl<TInstance, TChannel>();

			AddConfigurator(configurator);

			return configurator;
		}

		public void AddConfigurator(ChannelBuilderConfigurator<TChannel> configurator)
		{
			_configurator = configurator;
		}
	}


	public class InstanceConfiguratorImpl<TInstance, TChannel> :
		FiberFactoryConfiguratorImpl<InstanceConfigurator<TInstance, TChannel>>,
		InstanceConfigurator<TInstance, TChannel>,
		ChannelBuilderConfigurator<TChannel>
		where TInstance : class
	{
		Func<ChannelBuilder<TChannel>, ChannelProvider<TChannel>> _providerFactory;

		public InstanceConfiguratorImpl()
		{
			HandleOnCallingThread();
		}

		public void Configure(ChannelBuilder<TChannel> builder)
		{
			ChannelProvider<TChannel> provider = _providerFactory(builder);

			Fiber fiber = GetConfiguredFiber(builder);

			builder.AddChannel(fiber, x => new InstanceChannel<TChannel>(x, provider));
		}

		public void ValidateConfiguration()
		{
			if (_providerFactory == null)
				throw new ChannelConfigurationException(typeof(TChannel), "No instance provider was specified in the configuration");

			ValidateFiberFactoryConfiguration();
		}

		public void SetProviderFactory(Func<ChannelBuilder<TChannel>, ChannelProvider<TChannel>> providerFactory)
		{
			_providerFactory = providerFactory;
		}
	}
}