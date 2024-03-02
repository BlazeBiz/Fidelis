<template>
    <div>
        <h1>About</h1>
        <p>
            Welcome to Iron Marauder, a sample application. The demo is intended to illustrate some concepts and
            techniques around building a full-stack application. The major components of the stack are:
        </p>
        <ul>
            <li><span class="azure">Azure SQL database</span></li>
            <li><span class="product">.NET 6.0 MVC</span> API server deployed as an <span class="azure">Azure App
                    Service</span></li>
            <li>Vue client application</li>
            <ul>
                <li>Using <span class="product">Vue.js 3.0</span> with <span class="product">JavaScript</span></li>
                <li>Deployed as an <span class="azure">Azure Static Website</span></li>
            </ul>
        </ul>
        <h2>Source code</h2>
        <p>
            You can find the source code for this application in <a href="https://github.com/BlazeBiz/Marauder" target="_blank">
                this GitHub repository</a>. Code is under the <code>prototype</code> folder.
        </p>
        <p>
            There's a <code>README.md</code> at the top of the repo. This file documents the overall architecture
            and the concepts demonstrated. Application tier components are in the folders within <code>prototype</code>:
        </p>
        <ul>
            <li><code>MarauderServer</code> contains the .NET MVC API C# code.</li>
            <li><code>VikingDB</code> contains the code for the SQL Server database.</li>
            <li><code>Vue/marauder-vue</code> contains the Vue / JavaScript code for the client application.</li>
        </ul>
        <p>
            Each of these folders has its own <code>README.md</code> which documents that application tier in further
            detail.
        </p>
        <h2>A Note on the database</h2>
        <p>
            This application uses the Azure SQL Serverless Free tier. One of the features of this, which can't be disabled,
            is the <em>auto-pause</em> feature. If there is no activity against the database for an hour,
            Azure SQL <em>pauses</em> the database. The first time the application tries to open a connection after this,
            Azure SQL must <em>resume</em> the database. This can take up to a minute, so it typically results in a
            connection
            timeout error. The API has retry logic to recover from the timeout and connect, but <strong><em>you'll notice a
                    long delay
                    the first time the application hits the database after a time of inactivity</em></strong>.
        </p>
    </div>
</template>

<script>
export default {
    components: {
    },
    name: "HomeView",
    data() {
        return {
        };
    },
};
</script>

<style scoped>
.azure {
    font-style: italic;
    color: azure;
    background-color: darkblue;
}

.product {
    font-weight: bolder;
    color: darkblue;
}

p code,
ul code {
    background-color: beige;
    display: inline-block;
    padding-left: 5px;
    padding-right: 5px;
}
</style>