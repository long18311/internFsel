const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7136/chathub")
  .configureLogging(signalR.LogLevel.Information)
  .build();
const start = async () => {
  try {
    await connection.start();
    console.log("connected to server");
  } catch (error) {
    console.log(error);
  }
};
// getting the username from prompt modal and store in session
const joinUser = async () => {
  const name = window.prompt("Enter the name:  ");
  if (name) {
    sessionStorage.setItem("user", name);
    await joinChat(name);
  }
};
const joinChat = async (user) => {
  if (!user) return;
  try {
    const message = ` ${user} joined`;
    await connection.invoke("JoinChat", user, message);
  } catch (error) {
    console.log(error);
  }
  // await connection
  //   .invoke("JoinChat", user, message)
  //   .then(() => {
  //     console.log("Joined chat");
  //   })
  //   .catch((error) => console.log(error));
};
const getUser = () => sessionStorage.getItem("user");
const receiveMessage = async () => {
  const currentUser = getUser();
  if (!currentUser) return;
  try {
    await connection.on("ReceiveMessage", (user, message) => {
      const messageClass = currentUser === user ? "send" : "received";
      appendMessage(message, messageClass);
      const alertSound = new Audio("chat-sound.mp3");
      alertSound.play();
    });
  } catch (error) {
    console.log(error);
  }
};
const appendMessage = (message, messageClass) => {
  const messageSectionEl = document.getElementById("messageSection");
  const msgBoxEl = document.createElement("div");
  msgBoxEl.classList.add("msg-box");
  msgBoxEl.classList.add(messageClass);
  msgBoxEl.innerHTML = message;
  messageSectionEl.appendChild(msgBoxEl);
};
//event
document.getElementById("btnSend").addEventListener("click", async (e) => {
  e.preventDefault();
  const user = getUser();
  if (!user) return;
  const txtMessage = document.getElementById("txtMessage");
  const msg = txtMessage.value;
  if (msg) {
    await sendMessage(user, `${user}: ${msg}`);
    txtMessage.value = "";
  }
});
const sendMessage = async (user, message) => {
  try {
    await connection.invoke("SendMessage", user, message);
  } catch (error) {
    console.log(error);
  }
};
//
const startApp = async () => {
  await start();
  await joinUser();
  await receiveMessage();
  //
};
startApp();
