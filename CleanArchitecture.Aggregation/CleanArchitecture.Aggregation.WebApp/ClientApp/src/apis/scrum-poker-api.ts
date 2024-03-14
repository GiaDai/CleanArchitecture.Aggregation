import { Board } from '../@types/board';
import { UserModel } from '../@types/user';

import http from "../utils/http";

export const createBoard = (board: Board) => {
  return http.post<string>('/v1/scrum-poker/boards', board);
}

export const createUser = (boardId: string,user: UserModel) => {
  return http.post<string>(`/v1/boards/${boardId}/users`, user);
}

export const updateUserPoint = (boardId: string, user: UserModel) => {
  return http.put<boolean>(`/v1/boards/${boardId}/users`, user);
}

export const deleteUser = (boardId: string, userId: string) => {
  return http.delete<boolean>(`/v1/boards/${boardId}/users/${userId}`);
}