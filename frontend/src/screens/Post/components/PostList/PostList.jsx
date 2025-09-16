import React, { useMemo, useState } from "react";
import CustomTable from "@/components/CustomTable/CustomTable";
import { useDispatch, useSelector } from "react-redux";
import { pageSize } from "@/constants/common";
import postApiServices from "@/features/apis/posts/posts";
import {
  setCurrentPage,
  setEditDetail,
  setPages,
  setPosts,
  setSearchKey,
  setVerifyBtnDisabled,
} from "@/features/redux/slicers/postSlice";
import { Button } from "react-bootstrap";

const PostList = ({ loading, handleDelete, getPosts }) => {
  const header = useMemo(
    () => [
      { name: "Url", width: "30%" },
      { name: "Type", width: "10%" },
      { name: "Shared", width: "20%" },
      { name: "Updated Time", width: "20%" },
    ],
    []
  );

  const dispatch = useDispatch();
  const posts = useSelector((state) => state.post.posts);
  const totalPages = useSelector((state) => state.post.totalPages);
  const reduxSearchKey = useSelector((state) => state.post.searchKey);
  const currentPage = useSelector((state) => state.post.currentPage);
  const xUser = useSelector((state) => state.socialAccessInfo.userDetails.x);
  const facebookUser = useSelector(
    (state) => state.socialAccessInfo.userDetails.facebook
  );
  const isVerifyBtnDisabled = useSelector(
    (state) => state.post.isVerifyBtnDisabled
  );

  const handleCheckRetweet = async (post) => {
    try {
      dispatch(setVerifyBtnDisabled(true));
      const request = {
        url: post.url,
        type: post.type,
      };
      await postApiServices.checkRetweet(request);
      getPosts();
    } catch (ex) {
    } finally {
      dispatch(setVerifyBtnDisabled(false));
    }
  };

  const renderPost = (post) => {
    const user = post.type === "Facebook" ? facebookUser : xUser;
    return (
      <>
        <td className="text-overflow-ellipse max-width-100">
          <a href={post.url} target="_blank">
            {post.url}
          </a>
        </td>
        <td className="text-overflow-ellipse max-width-100">{post.type}</td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {(() => {
            if (!user?.userName) return "Sign in Request";
            return post.isShared ? (
              "Yes"
            ) : (
              <Button
                onClick={() => handleCheckRetweet(post)}
                disabled={isVerifyBtnDisabled}
              >
                Verify
              </Button>
            );
          })()}
        </td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {post.lastSavedTime}
        </td>
      </>
    );
  };

  const fetchDataByPaging = async (currentPage, searchValue = "") => {
    const searchText = searchValue ?? reduxSearchKey;
    const searchRequest = {
      searchKey: searchText,
      pageIndex: currentPage,
      pageSize: pageSize,
    };
    const result = await postApiServices.getPostsByPaging(searchRequest);
    dispatch(setPosts(result?.posts));
    dispatch(setPages(result?.totalPages));
    dispatch(setCurrentPage(currentPage));
  };

  const handleSearchChange = async (value) => {
    const firstPage = 1;
    dispatch(setSearchKey(value));
    fetchDataByPaging(firstPage, value);
  };

  const handleSetEditDetail = (id) => {
    const post = posts.find((post) => post.id === id);
    const editDetail = {
      id: id,
      url: post.url,
      type: post.type,
    };
    dispatch(setEditDetail(editDetail));
  };

  return (
    <CustomTable
      headerArray={header}
      handleDelete={handleDelete}
      handleSetEditDetail={handleSetEditDetail}
      itemArray={posts}
      renderBodyRow={renderPost}
      title={"List Post"}
      loading={loading}
      totalPages={totalPages}
      fetchDataByPaging={fetchDataByPaging}
      searchKey={reduxSearchKey}
      handleSearchChange={(e) => handleSearchChange(e.target.value)}
      currentPage={currentPage}
      setCurrentPage={() => dispatch(setCurrentPage(currentPage))}
      isSearchable={true}
    />
  );
};

export default PostList;
