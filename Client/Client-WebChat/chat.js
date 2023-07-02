"use strict";
let connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7194/signalHub")
  .configureLogging(signalR.LogLevel.Information)
  .build();

var modal = document.getElementById("myModal");
var span = document.getElementsByClassName("close")[0];
span.onclick = function () {
  modal.style.display = "none";
  if (gettoken() != getTokenFromCookie()) {
    startApp();
    return;
  }
};
const getUserLogin = async () => {
  if ((await getTokenFromCookie()) == null) {
    return null;
  }
  // console.log(sessionStorage.getItem("token"));
  // const jwt_decode = require("jwt-decode");
  const decoded = jwt_decode(await getTokenFromCookie());
  var user = {
    id: decoded[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
    ],
    userName:
      decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
    fullName:
      decoded[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata"
      ],
  };
  return user;
};

const getUserReceive = () => JSON.parse(sessionStorage.getItem("userReceive"));
const gettoken = () => JSON.parse(sessionStorage.getItem("token"));

//hiện thông báo thông báo
async function toastinfo(userReceive) {
  const main = document.getElementById("toast");
  if (main) {
    const toast = document.createElement("div");

    toast.onclick = async function (e) {
      if (e.target.closest(".toast__close")) {
        main.removeChild(toast);
      }
      if (gettoken() != getTokenFromCookie()) {
        startApp();
      } else {
        sessionStorage.setItem("userReceive", JSON.stringify(userReceive));
        appendMessage(userReceive);
      }
      // sessionStorage.setItem("userReceive", JSON.stringify(userReceive));
      // appendMessage(userReceive);
    };

    toast.classList.add("toast", "toast--info");
    toast.innerHTML = `
                    <div class="toast__icon">
                        <i class="fas fa-info-circle"></i>
                    </div>
                    <div class="toast__body">
                        <h3 class="toast__title">Tin Nhắn</h3>
                        <p class="toast__msg">Bạn có tin nhắn từ ${userReceive.fullName}</p>
                    </div>
                    <div class="toast__close">
                        <i class="fas fa-times"></i>
                    </div>
                `;
    main.appendChild(toast);
  }
}
// bắt đầu mở kết nối với thông bào với server
const start = async () => {
  // if (connection.state == "Connected") {
  //   connection.stop();
  // }

  await connection
    .start()
    .then(() => {
      console.log("Connected to server");
      // Gửi dữ liệu thông qua kết nối SignalR ở đây
    })
    .catch((error) => {
      console.log("Error connecting to server:", error);
    });
  console.log("startbd2");
  connection.onclose((error) => {
    console.log("Connection closed:", error);
  });
  console.log("startbd3");
};
/////////////token cookie///////////////
function setTokenCookie(token, expiration) {
  var d = new Date();
  d.setTime(d.getTime() + expiration * 60 * 60 * 1000);
  var expires = "expires=" + d.toUTCString();
  document.cookie = "token=" + token + ";" + expires + ";path=/";
}
// Hàm kiểm tra xem có cookie chứa token hay không
function hasTokenCookie() {
  const tokenCookieName = "token";
  return document.cookie
    .split(";")
    .some((cookie) => cookie.trim().startsWith(`${tokenCookieName}=`));
}

// Hàm lấy giá trị token từ cookie
async function getTokenFromCookie() {
  const tokenCookieName = "token";
  const cookies = document.cookie.split(";");
  for (let i = 0; i < cookies.length; i++) {
    const cookie = cookies[i].trim();
    if (cookie.startsWith(`${tokenCookieName}=`)) {
      return cookie.substring(tokenCookieName.length + 1);
    }
  }
  return null;
}
function deleteTokenCookie() {
  const tokenCookieName = "token";
  document.cookie =
    tokenCookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}
// Gọi hàm để xóa cookie có tên "token"
//deleteCookie('token');
function getCookieValue(cookieName) {
  var name = cookieName + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var cookieArray = decodedCookie.split(";");

  for (var i = 0; i < cookieArray.length; i++) {
    var cookie = cookieArray[i].trim();

    if (cookie.indexOf(name) === 0) {
      return cookie.substring(name.length, cookie.length);
    }
  }

  return null;
}
//hiển thị người nhắn tin
const setUser = async () => {
  console.log("setUser");
  const header = {
    "Content-Type": "application/json",
    Authorization: `Bearer ${await getTokenFromCookie()}`,
  };
  await axios
    .get(`https://localhost:7194/api/User/GetUserLst`, {
      headers: header,
    })
    .then(function (response) {
      console.log(response.data);
      const userSectionEl = document.getElementById("userSection");
      while (userSectionEl.hasChildNodes()) {
        userSectionEl.removeChild(userSectionEl.firstChild);
      }

      response.data.forEach((item) => {
        let userBoxEl = document.createElement("div");
        userBoxEl.classList.add("userbox");
        userBoxEl.innerHTML = item.fullName;
        //userBoxEl.id = item.id;
        userBoxEl.onclick = async () => {
          if (gettoken() != getTokenFromCookie()) {
            startApp();
          } else {
            sessionStorage.setItem("userReceive", JSON.stringify(item));
            appendMessage(item);
          }
        };
        userSectionEl.appendChild(userBoxEl);
        console.log("12345");
      });
    })
    .catch(function (error) {
      console.log(error);
    });
};
// Đăng nhập
const login = async () => {
  const txtUsername = document.getElementById("UsernameInput").value;
  const txtPassword = document.getElementById("PasswordInput").value;
  //   const userlogin = new Object();
  if (
    txtUsername == "" ||
    txtPassword == "" ||
    txtUsername == null ||
    txtPassword == null
  ) {
    return;
  }
  await axios
    .post("https://localhost:7194/api/User/Login", {
      Username: txtUsername,
      Password: txtPassword,
    })
    .then(async function (response) {
      //console.log(response.data);
      setTokenCookie(response.data, 2);

      // sessionStorage.setItem("userLogin", JSON.stringify(user));
      sessionStorage.setItem("token", response.data);
      //console.log(await getTokenFromCookie());
      console.log("123456789");
      //console.log(await getUserLogin());
      modal.style.display = "none";
      return true;
    })
    .catch(function (error) {
      console.log(error);
    });
};
//kiểm tra tin nhắn và thông báo
const receiveMessage = async () => {
  try {
    if (await connection.hasOwnProperty("ReceiveMessage")) {
      await connection.off("ReceiveMessage");
    }

    await connection.on("ReceiveMessage", async (userSender, userReceive) => {
      const currentUser = await getUserLogin();
      if (currentUser == null) {
        console.log("getUserLogin == null");
      }
      var userReceiveht = await getUserReceive();
      if (userReceiveht == null) {
        console.log("getUserReceive == null");
      }
      if (userSender.id != currentUser.id) {
        await toastinfo(userSender);
      }
      if (
        (userReceiveht != null && currentUser.id == userReceive.id) ||
        (currentUser.id == userSender.id && userReceiveht.id == userReceive.id)
      ) {
        appendMessage(userReceiveht);
      }
    });
  } catch (error) {
    console.log(error);
  }
};
//lấy tin nhắn
const appendMessage = async (userReceive) => {
  const header = {
    "Content-Type": "application/json",
    Authorization: `Bearer ${await getTokenFromCookie()}`,
  };
  await axios
    .post(
      `https://localhost:7194/api/Message?userReceiveid=${userReceive.id}`,
      null,
      {
        headers: header,
      }
    )
    .then(async function (response) {
      const userLogin = await getUserLogin();
      console.log(response.data);
      const messageSectionEl = document.getElementById("messageSection");
      while (messageSectionEl.hasChildNodes()) {
        messageSectionEl.removeChild(messageSectionEl.firstChild);
      }
      response.data.forEach((item) => {
        const msgBoxEl = document.createElement("div");
        msgBoxEl.classList.add("msg-box");
        if (item.userSenderid == userLogin.id) {
          msgBoxEl.classList.add("send");
          msgBoxEl.innerHTML = userLogin.fullName + ": " + item.content;
        } else {
          msgBoxEl.classList.add("received");
          msgBoxEl.innerHTML = userReceive.fullName + ": " + item.content;
        }
        messageSectionEl.appendChild(msgBoxEl);
      });
    })
    .catch(function (error) {
      console.log(error);
    });
};
// join chat
const joinChat = async () => {
  var user = await getUserLogin();
  console.log(user.id);
  if (!user) return;
  try {
    await connection.invoke("JoinChat", user.id);
  } catch (error) {
    console.log(error);
  }
};
// nút gửi tin nhắn
document.getElementById("btnSend").addEventListener("click", async (e) => {
  e.preventDefault();
  if (gettoken() != getTokenFromCookie()) {
    startApp();
    return;
  }
  const userLogin = await getUserLogin();
  const userReceive = await getUserReceive();
  if (!userLogin) return;
  if (!userReceive) return;
  const txtMessage = document.getElementById("txtMessage");
  const msg = txtMessage.value;
  if (msg) {
    await sendMessage(userLogin, msg, userReceive);
    txtMessage.value = "";
  }
});
//gửi tin nhắn
const sendMessage = async (userSender, message, userReceive) => {
  try {
    // const header = {
    //   Authorization: `Bearer ${getTokenFromCookie()}`,
    // };
    await connection.invoke(
      "SendMessage",
      userSender.id,
      message,
      userReceive.id
      // { headers: header }
    );
  } catch (error) {
    console.log(error);
  }
};

var loginbtn = document.getElementsByClassName("login")[0];
// nút đăng nhập
loginbtn.onclick = async () => {
  await login();
  if ((await getUserLogin()) != null) {
    await joinChat();
    await receiveMessage();
    await setUser();
  }
};
document.getElementById("dangxuat-i").addEventListener("click", async (e) => {
  deleteTokenCookie();
  let userSectionEl = document.getElementById("userSection");
  while (userSectionEl.hasChildNodes()) {
    userSectionEl.removeChild(userSectionEl.firstChild);
  }
  modal.style.display = "block";
  e.preventDefault();
});
//khởi tạo
const startApp = async () => {
  // console.log("token:" + getTokenFromCookie());
  if (hasTokenCookie()) {
    sessionStorage.setItem("token", await getTokenFromCookie());
    modal.style.display = "none";
    await joinChat();
    await receiveMessage();
    await setUser();
  } else {
    modal.style.display = "block";
  }
};
await start();
startApp();
