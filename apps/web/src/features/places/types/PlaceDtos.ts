export type PlaceDto = {
  id: string;
  ownerUserId: string;
  name: string;
  description?: string | null;
  createdAt: string;
};

export type CreatePlaceRequest = {
  name: string;
  description?: string | null;
};
