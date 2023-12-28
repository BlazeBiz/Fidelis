// import DOMPurify from 'dompurify';
// import { marked } from 'marked';
export default {
    PERMISSIONS: {
        None: 0,
        View: 1,
        Change: 2,
        Manage: 4,
        Administer: 8,
    },
    dateOptions: {
        year: 'numeric',
        month: 'numeric',
        day: '2-digit',
    },
    timeOptions: {
        hour: 'numeric',
        minute: '2-digit',
        second: '2-digit',
        hour12: true,
    },
    colorDefaults: {
        "--body-bg-1": "#ffffff",
        "--body-bg-2": "#dddddd",
        "--heading-fg": "#00bfff",
        "--body-text-fg": "#383838",
        "--footer-bg": "#5e5e5e",
        "--footer-fg": "#ffffff",
        "--editor-bg": "#e3e3e3",
        "--editor-fg": "#969696",
        "--action-bg": "rgba(128,128,128,0.2)",
        "--action-fg": "#00b0f0",
        "--table-heading-bg": "#00b0f0",
        "--table-heading-fg": "#ffffff",
        "--table-bg-1": "#f0f0f0",
        "--table-bg-2": "#e1e1e1",
        "--table-cell-fg": "#000000",
        "--table-shadow-fg": "#ffffff",
        "--table-gridlines-fg": "#7d7d7d",
        "--link-fg": "#00bfff",
        "--sidenav-bg-1": "#d6d6d6",
        "--sidenav-bg-2": "#c2c2c2",
        "--sidenav-fg": "#7d7d7d"
      },
    
    // asHTML(md) {
    //     if (md) {
    //         let html = marked.parse(md);
    //         return DOMPurify.sanitize(html);
    //     }
    // },

    formatDate(date) {
        // This may be where the problem is...when we create the date, what timezone is it?
        if (!date) return "";
        let d = new Date(date);
        // let d = new Date(Date.parse(date + "+00:00"));
        return d.toLocaleDateString([], this.dateOptions);
    },

    formatDateTime(date) {
        if (!date) return "";
        // let d = new Date(date);
        let d = new Date(Date.parse(date + "+00:00"));
        return d.toLocaleDateString([], this.dateOptions) + " " + d.toLocaleTimeString([], this.timeOptions);
    },

    isoDate(date) {
        return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(
            2,
            "0"
        )}-${String(date.getDate()).padStart(2, "0")}`;
    },
    
    isNumeric(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },

    /**
     * Calculate "how long ago" the date given was in a user-friendly format
     * @param {date} fromDate 
     */
    ago(fromDate) {
        if (!fromDate) return "";
        fromDate = new Date(fromDate);
        const now = new Date().getTime();
        const from = fromDate.getTime();
        let diff = Math.floor((now - from) / (1000 * 60)); // minutes

        if (diff < 3) {
            return "seconds";
        }

        if (diff < 60) {
            return "minutes";
        }

        diff = Math.floor( diff / 60 );  // Change to hours
        if (diff < 2) {
            return "1 hour"; 
        }


        if (diff < 24) {
            return `${diff} hours`;
        }

        diff = Math.floor( diff / 24 );  // Change to days

        if (diff < 2) {
            return "1 day";
        }
        return `${diff} days`;
    },

    /**
     * Tests whether the current user has a required permission (in its userPermissions property) 
     * @param {Object} obj A Portfolio or Project object to test permissions on
     * @param {Permissions} permissionRequired A property from the PERMISSIONS object for permission to test for
     * @returns True if the user has the required permission
     */
    hasPermission(obj, permissionRequired) {
        if (!obj) return false;
        return (obj.userPermissions & permissionRequired) > 0;
    },
    applyColors(colorOverrides) {
        // Override css colors with local definitions
        let style = document.querySelector(":root").style;
        for (let prop in this.colorDefaults) {
            if (colorOverrides && colorOverrides[prop]) {
                style.setProperty(prop, colorOverrides[prop]);
            } else {
                style.setProperty(prop, this.colorDefaults[prop]);
            }
        }
    },
    resetColors() {
        this.applyColors(null);
    },

    /**
     * This function generates a 28-character base64 string to be used as an access token
     * for read-only access to a portfolio or a project.
     * For longer or shorter tokens, mess with the length of array or its type 
     * (Int8Array, a Uint8Array, an Int16Array, a Uint16Array, an Int32Array, or a Uint32Array).
     * See https://developer.mozilla.org/en-US/docs/Web/API/Crypto/getRandomValues
     * 
     * If there is ever a need to translate back to the original numbers, use atob().
     */
    generateAccessToken() {
        // Get an array of random numbers
        let array = new Uint32Array(2);

        // getRandomNumbers fills in the array
        window.crypto.getRandomValues(array);

        // convert to a base64 string
        let base64 = btoa(array);
        return base64;
    }
}

