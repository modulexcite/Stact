// Copyright 2010 Chris Patterson
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
namespace Stact
{
	using Magnum.Collections;


	public class KeyedChannelProvider<TChannel, TKey> :
		ChannelProvider<TChannel>
	{
		readonly Cache<TKey, Channel<TChannel>> _dictionary;
		readonly KeyAccessor<TChannel, TKey> _keyAccessor;

		public KeyedChannelProvider(ChannelProvider<TChannel> channelProvider, KeyAccessor<TChannel, TKey> keyAccessor)
		{
			ChannelProvider = channelProvider;
			_keyAccessor = keyAccessor;

			_dictionary = new Cache<TKey, Channel<TChannel>>();
		}

		public ChannelProvider<TChannel> ChannelProvider { get; private set; }

		public Channel<TChannel> GetChannel(TChannel message)
		{
			TKey key = _keyAccessor(message);

			return _dictionary.Retrieve(key, x => ChannelProvider.GetChannel(message));
		}
	}
}