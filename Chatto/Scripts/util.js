$(function () {

	var hubVar = $.connection.signalHub;

	hubVar.client.showNotification = function (userName, message) {
		document.getElementById('notificationBlock').removeAttribute('hidden');
		document.getElementById('notificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));
		$.fn.playSound();

		setTimeout(function () {
			window.location.reload();
		}, 2000);
	};

	$.fn.playSound = function () {
		const audio = new Audio("/Sounds/ping.wav");
		audio.play();
	}

	hubVar.client.onNewUserConnected = function (id, name) {
		AddUser(id, name);
	}

	hubVar.client.onConnected = function (id, userName, allUsers) {

		for (i = 0; i < allUsers.length; i++) {

			AddUser(allUsers[i].ConnectionId, allUsers[i].User);
		}
	}

	hubVar.client.onUserDisconnected = function (id, userName) {

		$('#' + id).remove();
	}

	$.connection.hub.start().done(function () {

		console.log('connected! ' + hubVar.connection.id);

		hubVar.server.connect(document.getElementById('userName').getAttribute('value'));

		$('#addFriendNotification').click(function () {
			hubVar.server.sendNotification(document.getElementById('friendUserName').getAttribute('value'), 'Someone added you to their friendslist! Reloading page in 2 seconds');
		});

		$('#removeFriendNotification').click(function () {
			hubVar.server.sendNotification(document.getElementById('friendUserName').getAttribute('value'), 'Someone removed you from their friendslist! Reloading page in 2 seconds');
		});
	})

	function htmlEncode(value) {
		var encodedValue = $('<div />').text(value).html();
		return encodedValue;
	}

	function AddUser(id, name) {

		var userId = $('#hdId').val();

		if (userId != id) {

			$("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
		}
	}
})