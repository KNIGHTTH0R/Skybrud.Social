using Skybrud.Social.Facebook.Endpoints.Raw;
using Skybrud.Social.Facebook.Options.Posts;
using Skybrud.Social.Facebook.Responses.Posts;

namespace Skybrud.Social.Facebook.Endpoints {
    
    public class FacebookPostsEndpoint {

        #region Properties

        /// <summary>
        /// Gets a reference to the Facebook service.
        /// </summary>
        public FacebookService Service { get; private set; }

        /// <summary>
        /// Gets a reference to the raw endpoint.
        /// </summary>
        public FacebookPostsRawEndpoint Raw {
            get { return Service.Client.Posts; }
        }

        #endregion

        #region Constructors

        internal FacebookPostsEndpoint(FacebookService service) {
            Service = service;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets information about the post with the specified <code>id</code>.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        public FacebookPostResponse GetPost(string id) {
            return FacebookPostResponse.ParseResponse(Raw.GetPost(id));
        }

        /// <summary>
        /// Gets a list of posts of the user or page with the specified <code>identifier</code>.
        /// </summary>
        /// <param name="identifier">The identifier (ID or name) of the page or user.</param>
        public FacebookPostsResponse GetPosts(string identifier) {
            return GetPosts(identifier, new FacebookPostsOptions());
        }

        /// <summary>
        /// Gets a list of posts of the user or page with the specified <code>identifier</code>.
        /// </summary>
        /// <param name="identifier">The identifier (ID or name) of the page or user.</param>
        /// <param name="limit">The maximum amount of posts7 to return.</param>
        public FacebookPostsResponse GetPosts(string identifier, int limit) {
            return GetPosts(identifier, new FacebookPostsOptions {
                Limit = limit
            });
        }

        /// <summary>
        /// Gets a list of posts of the user or page with the specified <code>identifier</code>.
        /// </summary>
        /// <param name="identifier">The identifier (ID or name) of the page or user.</param>
        /// <param name="options">The options for the call to the API.</param>
        public FacebookPostsResponse GetPosts(string identifier, FacebookPostsOptions options) {
            return FacebookPostsResponse.ParseResponse(Raw.GetPosts(identifier, options));
        }

        #endregion

    }

}