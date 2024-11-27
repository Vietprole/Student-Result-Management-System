import API_BASE_URL from "./base-url";
import axios from 'axios';

const API_LOPHOCPHAN = `${API_BASE_URL}/api/lophocphan`;

export const getAllLopHocPhans = async () => {
  try {
    const response = await axios.get(API_LOPHOCPHAN);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};

export const getLopHocPhanById = async (id) => {
  try {
    const response = await axios.get(`${API_LOPHOCPHAN}/${id}`);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};

export const addLopHocPhan = async (newData) => {
  try {
    const response = await axios.post(API_LOPHOCPHAN, newData);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};

export const updateLopHocPhan = async (id, updatedData) => {
  try {
    const response = await axios.put(`${API_LOPHOCPHAN}/${id}`, updatedData);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};

export const deleteLopHocPhan = async (id) => {
  try {
    const response = await axios.delete(`${API_LOPHOCPHAN}/${id}`);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};

export const getSinhViensByLopHocPhanId = async (lopHocPhanId) => {
  console.log("lopHocPhanId: ", lopHocPhanId);
  try {
    const response = await axios.get(`${API_LOPHOCPHAN}/${lopHocPhanId}/view-sinhviens`);
    return response.data;
  } catch (error) {
    console.log("error message: ", error.message);
  }
};
