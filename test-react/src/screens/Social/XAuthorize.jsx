import React from "react";
import { auth } from "@/config/firebaseConfig";
import { TwitterAuthProvider, signInWithPopup } from "firebase/auth";
import { useDispatch, useSelector } from "react-redux";
import { setUserDetail } from "@/features/redux/slicers/socialAccessInfoSlice.js";
import { Button, Container } from "react-bootstrap";
import socialAccessInfoApiServices from "@/features/apis/socialAccessInfo/socialAccessInfo";
import { socialName } from "@/constants/socialConstant";

const XAuthorize = ({getPosts, getUserSocialAccessInfo}) => {
  const xProvider = new TwitterAuthProvider();
  const dispatch = useDispatch();
  const xUser = useSelector((state) => state.socialAccessInfo.userDetails.x);

  const handleSignIn = async () => {
    try {
      const result = await signInWithPopup(auth, xProvider);
      console.log('result', result);
      const userDetail = result._tokenResponse;
      const user = {
        userName: userDetail.screenName,
      };
      dispatch(setUserDetail({ platform: socialName.X, data: user }));
      const authRequest= {
        accessToken: userDetail.oauthAccessToken,
        accessSecret: userDetail.oauthTokenSecret,
        type: socialName.X,
        userName: userDetail.screenName,
        userId: userDetail.federatedId
      }
      socialAccessInfoApiServices.create(authRequest);
      getPosts();
    } catch (error) {
      console.error("X Sign-In Error", error);
    }
  };

  const handleSignOut = async () => {
    await socialAccessInfoApiServices.delete(xUser.id);
    getUserSocialAccessInfo();
  }

  return (
    <Container>
      <h4>Account Detail</h4>
      {xUser?.userName ? (
        <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
          <p style={{marginBottom: 0}}>X:</p>
          <p style={{marginBottom: 0}}>Welcome, {xUser.userName}</p>
          <Button variant="danger" onClick={handleSignOut}>X</Button>
        </div>
      ) : (
        <Button onClick={handleSignIn}>Sign in with X</Button>
      )}
    </Container>
  );
};

export default XAuthorize;
