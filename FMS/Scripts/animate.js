// JavaScript Document
jQuery(document).ready(function() {

	jQuery('.bounce-in-left').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInLeft', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.bounce-in-down').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInDown', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.bounce-in-right').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.fade-in').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated fadeIn', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.fade-in-up').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated fadeInUp', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.bounce-in-up').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInUp', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.slide-in-up').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated slideInUp', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.slide-in-left').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated slideInLeft', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.slide-in-down').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated slideInDown', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.slide-in-right').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated slideInRight', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.rotate-in').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated rotateIn', // Class to add to the elements when they are visible
	    offset: 200, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.rotate-in-up-left').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated rotateInUpLeft', // Class to add to the elements when they are visible
	    offset: 200, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.flip-a').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated flip', // Class to add to the elements when they are visible
	    offset: 200, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.flip-in-x').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated flipInX', // Class to add to the elements when they are visible
	    offset: 200, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.bounce-in-down2').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInDown', // Class to add to the elements when they are visible
	    offset: 180, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.bounce-in-right2').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 150, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.bounce-in-right3').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 170, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.bounce-in-right4').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 200, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
	
	jQuery('.bounce-in-right5').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 220, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function(elem, action){}, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.rotate-in2').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated rotateIn', // Class to add to the elements when they are visible
	    offset: 100, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function (elem, action) { }, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.bounce-in-left2').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInLeft', // Class to add to the elements when they are visible
	    offset: 50, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function (elem, action) { }, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});

	jQuery('.bounce-in-right2').addClass("animatehidden").viewportChecker({
	    classToAdd: 'animatevisible animated bounceInRight', // Class to add to the elements when they are visible
	    offset: 50, // The offset of the elements (let them appear earlier or later)
	    repeat: true, // Add the possibility to remove the class if the elements are not visible
	    callbackFunction: function (elem, action) { }, // Callback to do after a class was added to an element. Action will return "add" or "remove", depending if the class was added or removed
	    scrollHorizontal: false // Set to true if your website scrolls horizontal instead of vertical.
	});
});