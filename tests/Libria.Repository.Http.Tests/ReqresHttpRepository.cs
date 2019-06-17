namespace Libria.Repository.Http.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Core.Specification;
	using Repository;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	public class ReqresHttpRepository : HttpRepository<User>
	{
		internal class GetUserResponse
		{
			public User Data { get; set; }
		}

		public ReqresHttpRepository(HttpContext httpContext) : base(httpContext)
		{
		}

		public override async Task<User> GetByIdAsync(params object[] keyValues)
		{
			return await GetByIdAsync(keyValues, default(CancellationToken));
		}

		public override async Task<User> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken)
		{
			var requestUrl = $"https://reqres.in/api/users/{keyValues[0]}";

			using (var httpClient = HttpContext.GetHttpClient())
			{
				var responseString = await httpClient.GetStringAsync(requestUrl);
				var userResponse = JsonConvert.DeserializeObject<GetUserResponse>(responseString);

				return userResponse.Data;
			}
		}

		public override async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			var requestUrl = $"https://reqres.in/api/users";

			using (var httpClient = HttpContext.GetHttpClient())
			{
				DefaultContractResolver contractResolver = new DefaultContractResolver
				{
					NamingStrategy = new CamelCaseNamingStrategy()
				};

				var serializedBody = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
				{
					ContractResolver = contractResolver,
					Formatting = Formatting.Indented
				});

				var response = await httpClient.PostAsync(requestUrl, new StringContent(serializedBody, Encoding.UTF8, "application/json"), cancellationToken);
				var content = await response.Content.ReadAsStringAsync();
				var resultUser = JsonConvert.DeserializeObject<User>(content);

				HttpContext.AddRollbackAction(async (ct) => await RemoveAsync(resultUser, true, ct));
				return JsonConvert.DeserializeObject<User>(content);
			}
		}

		public override Task<User> UpdateAsync(User entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public override Task AddRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public override async Task<User> RemoveAsync(User entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await RemoveAsync(entity, false, cancellationToken);
		}

		private async Task<User> RemoveAsync(User entity, bool isRollbackAction, CancellationToken cancellationToken)
		{
			var requestUrl = $"https://reqres.in/api/users/{entity.Id}";

			using (var httpClient = HttpContext.GetHttpClient())
			{
				await httpClient.DeleteAsync(requestUrl, cancellationToken);

				if (!isRollbackAction)
				{
					HttpContext.AddRollbackAction(async (ct) => await AddAsync(entity, ct));
				}

				return entity;
			}
		}

		public override Task RemoveRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public override Task<int> CountAsync(ISpecification<User> specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public override Task<User> FindAsync(ISpecification<User> specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public override Task<IEnumerable<User>> FindAllAsync(ISpecification<User> specification, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}
}

