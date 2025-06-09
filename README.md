# YeutzLi
**YeutzLi** is an AI-powered relationship counseling app built with Xamarin.Android. Powered by a locally-hosted [Ollama](https://ollama.com/) LLaMA-based chatbot to provide guidance and interactivity.

---

## ✨ Features

- **Counseling Mode**  
  Chat privately with an AI counselor trained to provide helpful advice and support for relationship issues.

- **Commonground Mode**  
  A unique chat mode designed to help two users find shared understanding and resolve disagreements, guided by the chatbot.

- **Firebase Authentication**  
  Secure login system powered by Firebase, allowing users to register, log in, and manage their sessions.

- **Local Ollama API Integration**  
  All AI responses are generated using a local instance of LLaMA via Ollama for privacy and offline use.

- **Material Design UI**  
  A smooth and modern user experience, following Google’s Material Design guidelines.

       
![chatuml-diagram](https://github.com/user-attachments/assets/2bde8153-2929-496c-9d04-a1aff6e201c5)

---

## 🛠️ Technologies Used

- **Xamarin.Android (C#, XML)**
- **Firebase Authentication**
- **Ollama (LLaMA-based language model)**
- **Material Design Components**

---

## 📸 Screenshots

*(Add screenshots here to showcase the Counseling and Commonground modes, authentication flow, and UI design)*
<div style="display: flex; gap: 10px;">
  <img src="https://github.com/user-attachments/assets/3ca6edd8-1801-4adc-b786-2c045b119ebf" alt="YeutzLi Screenshot 3" width="200"/>
  <img src="https://github.com/user-attachments/assets/ea1111f9-e6d1-4b3e-a672-e404f1db5986" alt="YeutzLi Screenshot 2" width="200"/>
  <img src="https://github.com/user-attachments/assets/1ea1563f-e1c4-439b-893f-9084b9782d9a" alt="YeutzLi Screenshot 1" width="200"/>

</div>



## Setup

1. Create an Ollama API endpoint
2. Change the current ``desktop`` in the CounselorActivity.cs and CommongroundActivity.cs to your server's IP
3. Rebuild the app
