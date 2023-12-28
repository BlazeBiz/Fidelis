import axios from 'axios'
const LAG_TIME = parseInt(import.meta.env.VITE_APP_API_LAG_MS);     // Milliseconds of "lag" for api calls
export default {
    baseUrl: import.meta.env.VITE_MARAUDER_API,

    /************************************************************
     * getCustomers
     ***********************************************************/
    searchCustomers(searchField, searchType, searchValue) {
        return this.httpGetPromise(`${this.baseUrl}/customers?searchField=${searchField}&searchType=${searchType}&searchValue=${searchValue}`);
    },

    /************************************************************
     * getCustomerById
     ***********************************************************/
    getCustomerById(customerId) {
        return this.httpGetPromise(`${this.baseUrl}/customers/${customerId}`);
    },
    /************************************************************
     * updateCustomer
     ***********************************************************/
    updateCustomer(customer) {
        return this.httpPutPromise(`${this.baseUrl}/customers/${customer.customerId}`, customer);
    },






    /************************************************************
     * getProjectByIdAndLink
     ***********************************************************/
    getProjectByIdAndLink(projectId, portfolioLink) {
        return this.httpGetPromise(`${this.baseUrl}/project?id=${projectId}&link=${portfolioLink}`);
    },

    /************************************************************
     * getDashboard - Gets the portfolios and projects this user can see
     ***********************************************************/
    getDashboard(orgId) {
        return this.httpGetPromise(`${this.baseUrl}/dashboard?orgId=${orgId}`);
    },

    /************************************************************
     * getPortfolioById
     ***********************************************************/
    getPortfolioById(portfolioId) {
        return this.httpGetPromise(`${this.baseUrl}/portfolios/${portfolioId}`);
    },

    /************************************************************
     * getPortfolioByLink
     ***********************************************************/
    getPortfolioByLink(portfolioLink) {
        return this.httpGetPromise(`${this.baseUrl}/portfolio/${portfolioLink}`);
    },

    /************************************************************
     * addPortfolio
     ***********************************************************/
    addPortfolio(newPortfolio) {
        return this.httpPostPromise(`${this.baseUrl}/portfolios`, newPortfolio);
    },


    /************************************************************
     * addPersonalOrganization
     ***********************************************************/
    addPersonalOrganization() {
        return this.httpPostPromise(`${this.baseUrl}/organizations?type=personal`, {});
    },

    /************************************************************
     * getThemes - Gets all themes
     ***********************************************************/
    getThemes() {
        return this.httpGetPromise(`${this.baseUrl}/themes`);
    },
    /************************************************************
     * updateTheme
     ***********************************************************/
    updateTheme(theme) {
        return this.httpPutPromise(`${this.baseUrl}/themes/${theme.themeId}`, theme);
    },
    /************************************************************
     * addTheme
     ***********************************************************/
    addTheme(theme) {
        return this.httpPostPromise(`${this.baseUrl}/themes`, theme);
    },

    /************************************************************
     * addProject
     ***********************************************************/
    addProject(newProject) {
        return this.httpPostPromise(`${this.baseUrl}/projects`, newProject);
    },

    /************************************************************
     * updateProject
     ***********************************************************/
    updateProject(project) {
        return this.httpPutPromise(`${this.baseUrl}/projects/${project.projectId}?updateType=edit`, project);
    },

    /************************************************************
     * updateManageProject
     ***********************************************************/
    updateManageProject(project) {
        return this.httpPutPromise(`${this.baseUrl}/projects/${project.projectId}?updateType=manage`, project);
    },

    /************************************************************
     * Generic GET (with Promise)
     ***********************************************************/
    httpGetPromise(urlToFetch) {
        const lagPromise = this.setLag(LAG_TIME);
        return new Promise((resolve, reject) => {
            // this.acquireToken().then((accessToken) => {
            //     // console.log(`Access token: ${accessToken}`);
            //     this.setAuthToken(accessToken);
            // Perform the get
            axios.get(urlToFetch)
                .then(async function (response) {
                    await lagPromise;
                    resolve(response);
                }).catch(err => {
                    let msg = this.getErrorMessage(err, urlToFetch);
                    reject({ msg: msg, err: err });
                });
            //     }).catch( // catch for acquireToken
            //         (msg, error) => {
            //             console.log(msg + error);
            //             reject(msg, error);
            //         });
        });
    },

    /************************************************************
     * Generic POST (with Promise)
     ***********************************************************/
    httpPostPromise(urlToFetch, body) {
        const lagPromise = this.setLag(LAG_TIME);
        return new Promise((resolve, reject) => {
            // this.acquireToken().then((accessToken) => {
            //     // console.log(`Access token: ${accessToken}`);
            //     this.setAuthToken(accessToken);
            // Perform the post
            axios.post(urlToFetch, body)
                .then(async function (response) {
                    await lagPromise;
                    resolve(response);
                }).catch(err => {
                    let msg = this.getErrorMessage(err, urlToFetch);
                    reject({ msg: msg, err: err });
                });
            // }).catch( // catch for acquireToken
            //     (msg, error) => {
            //         console.log(msg + error);
            //         reject(msg, error);
            //     });
        });
    },

    /************************************************************
     * Generic PUT (with Promise)
     ***********************************************************/
    httpPutPromise(urlToFetch, body) {
        const lagPromise = this.setLag(LAG_TIME);
        return new Promise((resolve, reject) => {
            // this.acquireToken().then((accessToken) => {
            //     // console.log(`Access token: ${accessToken}`);
            //     this.setAuthToken(accessToken);
            // Perform the put
            axios.put(urlToFetch, body)
                .then(async function (response) {
                    await lagPromise;
                    resolve(response);
                }).catch(err => {
                    let msg = this.getErrorMessage(err, urlToFetch);
                    reject({ msg: msg, err: err });
                });
            // }).catch( // catch for acquireToken
            //     (msg, error) => {
            //         console.log(msg + error);
            //         reject(msg, error);
            //     });
        });
    },

    /************************************************************
     * Generic DELETE (with Promise)
     ***********************************************************/
    httpDeletePromise(urlToFetch) {
        const lagPromise = this.setLag(LAG_TIME);
        return new Promise((resolve, reject) => {
            // this.acquireToken().then((accessToken) => {
            //     // console.log(`Access token: ${accessToken}`);
            //     this.setAuthToken(accessToken);
            // Perform the delete
            axios.delete(urlToFetch)
                .then(async function (response) {
                    await lagPromise;
                    resolve(response);
                }).catch(err => {
                    let msg = this.getErrorMessage(err, urlToFetch);
                    reject({ msg: msg, err: err });
                });
            // }).catch( // catch for acquireToken
            //     (msg, error) => {
            //         console.log(msg + error);
            //         reject(msg, error);
            //     });
        });
    },

    /************************************************************
     * A function to create a "minimum delay" timeout promise.
     * This makes all async calls in this library take a minimum
     * amount of time, simulating a network when local, but not 
     * adding delay when in the cloud.
     ***********************************************************/
    setLag(milliseconds) {
        return new Promise((resolve) => {
            setTimeout(resolve, milliseconds);
        });
    },

    /************************************************************
     * Generic Error Handling
     ***********************************************************/
    // handleError(error, url, errorCallback) {
    //     let msg = this.getErrorMessage(error, url);
    //     if (errorCallback) {
    //         errorCallback(msg, error);
    //     } else {
    //         throw error;
    //     }
    // },
    getErrorMessage(error, url) {
        let msg;
        if (error.response) {
            if (error.response.status === 404) {
                msg = `Could not find resource: '${url}'.`;
            }
            else if (error.response.status === 403) {
                msg = 'The user does not have permission to perform this action.';
            }
            // TODO: Look for specific response codes for more helpful messages
            else {
                msg = `The server reported error code ${error.response.status}: ${error.response.statusText}`;
            }
        } else if (error.request) {
            msg = `The request was sent to ${url}, but the server could not be reached.`;
        } else {
            msg = `There was an error creating the request to ${url}.`;
        }
        console.error(msg);
        return msg;
    }
}